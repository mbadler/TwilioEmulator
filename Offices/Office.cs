using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwilioEmulator.Code;
using TwilioEmulator.Code.Extensions;
using TwilioEmulator.Phones;
using Twilio;
using System.Collections.Specialized;
using System.Net;

namespace TwilioEmulator.Offices
{
    public class Office:OfficeBase
    {

      

        public List<CallInstance> CallList = new List<CallInstance>();


        #region API Requests
        public CallInstance NewCallRequest(CallOptions co)
        {
            CallInstance c = new CallInstance()
            {
                CallForCreate = new Call()
                {
                    Sid = Guid.NewGuid().ToString().Replace("-", ""),
                    Status = TwilioCallStatuses.QUEUED,
                    Direction = "outbound-api",
                    DateCreated = DateTime.Now.ToUniversalTime(),
                    DateUpdated = DateTime.Now.ToUniversalTime(),
                    To = co.To,
                    From = co.From
                    
                },
                CallDirection = CallDirection.Out,
                CallOptions = co
               
            };

            CallList.Add(c);
            return c;
        }

        internal void HangupCallRequest(CallInstance d, string stat)
        {
            HangUpThePhoneConnectionFromTheserver(d, stat);
        }
        #endregion

        protected override void Process()
        {
            // run thru each call and see what needs to be done with them
       
            CallList.Where(x=>
                x.CallStatus==CallStatus.ReadyForProcessing||
                x.CallStatus==CallStatus.Queued||
                x.CallStatus==CallStatus.WaitingRinging
                ).AsParallel().ForAll(x=>
                {
                    ProcessCallInstance(x);
                });
            
        }

        void ProcessCallInstance(CallInstance ci)
        {
            if (ci.CallStatus == CallStatus.Queued && ci.CallDirection == CallDirection.Out)
            {
               // start the phone call
                ProcessCallPhone(ci);
            }

            if (ci.CallStatus == CallStatus.ReadyForProcessing)
            {
                // we are processing twiml
                ci.FlowEngine.StartCallFlow();

            }
            
        }

        

        private void ProcessCallPhone(CallInstance ci)
        {
            // when we go to multi phones we will look up by phone number etc... - but for now 
            // we will look at the first phone
            // if there is no first phone then fire the need phone eventhandler
            
            Boolean Callok = false;

            
            var c = ci.CallForGet;
            var v = SystemController.Instance.PhoneManager.AttemptDial(ci.CallForGet.To);
                 SystemController.Instance.Logger.LogObj(new  LogObj()
            {
                Caption = "Dial to Phone",
                LogSymbol = LogSymbol.TwilioToPhone,
                CallInstance = ci

            }.AddNode("Incoming Number", ci.CallOptions.To).AddNode("Phone Status", v.ToString()));
            switch (v)
            {

                case PhoneStatus.Busy:
                    {
                        ci.CallForSet.Status = TwilioCallStatuses.BUSY;

                        break;
                    }
                case PhoneStatus.Ringing:
                    {
                        Callok = true;
                        ci.CallForSet.Status = TwilioCallStatuses.RINGING;
                        break;
                    }
                case PhoneStatus.NotInService:
                    {
                        ci.CallForSet.Status = TwilioCallStatuses.FAILED;
                        break;
                    }
                case PhoneStatus.NoAnswer:
                    {
                        ci.CallForSet.Status = TwilioCallStatuses.NOANSWER;
                        break;
                    }
                default:
                    break;
            }
            // if it is not a good call send back a error call status and mark the call done
            // else mark it as waiting for the phone
            if (!Callok)
            {
                MarkCallEnded(ci);
                SendCallEndStatus(ci, ci.GenerateCallBackValue(), c.Status);
            }
            else
            {
                ci.CallStatus = CallStatus.WaitingRinging;
            }
            
        }

        public void PhoneHungUp(string phonenumber)
        {
            var a = GetCallInstanceFromPhoneNumber(phonenumber);
            var v = new LogObj()
            {
                Caption = "X Phone hung up call",
                LogSymbol = Code.LogSymbol.PhoneToTwilio,
                CallInstance = a
            };


            SystemController.Instance.Logger.LogObj(v);
            MarkCallEnded(a);
            SendCallEndStatus(a, a.GenerateCallBackValue(), "Hung up");
            
        }


        public void MarkCallEnded(CallInstance ci)
        {
            var a = ci.CallForSet;
            if (a.Status == TwilioCallStatuses.INPROGRESS)
            {
                a.EndTime = DateTime.Now;
                a.Duration = (DateTime.Now - a.DateCreated).Seconds;
            }
            a.Status = TwilioCallStatuses.COMPLETED;
           ci.CallStatus = CallStatus.Ended;

            
        }


        public void PhonePickedUp(string phonenumber, bool IsMachine)
        {
            // find the ci with that phone number
            var a = GetCallInstanceFromPhoneNumber(phonenumber);
            var v = new LogObj()
            {
                Caption = "^ Phone Picked Up " + (IsMachine ? " Machine" : ""),
                LogSymbol = LogSymbol.PhoneToTwilio,
                CallInstance = a
            };

            SystemController.Instance.Logger.LogObj(v);

            if (a.CallOptions.IfMachine != "")
            {
                if (!IsMachine)
                {
                    a.CallForSet.AnsweredBy = "Human";
                }
                else
                {
                    a.CallForSet.AnsweredBy = "Machine";
                    if (a.CallOptions.IfMachine == "Hangup")
                    {
                        HangUpThePhoneConnectionFromTheserver(a, "Answered by machine");
                        SendCallEndStatus(a, a.GenerateCallBackValue(), "Answered by answering machine");
                        
                        return;
                    }

                }
            }


            a.CallStatus = CallStatus.ReadyForProcessing;
            //ProcessDoTwimlRequest(a, a.CallOptions.Url, "Get Welcome message Twiml");

        }

       

        private void HangUpThePhoneConnectionFromTheserver(CallInstance ci,string Reason)
        {
            var v = new LogObj()
            {
                Caption = "X Server hung up call " + Reason,
                LogSymbol = Code.LogSymbol.TwilioToPhone,
                CallInstance = ci
            }.LogIt();

            MarkCallEnded(ci);
            SystemController.Instance.PhoneManager.CallHungUp(ci.CallOptions.To, Reason);

        }


        protected string SendCallEndStatus(CallInstance ci, NameValueCollection nvc,string Reason)
        {
            ci.CallStatus = CallStatus.CallbackSent;
            var a = "";

            var v = new LogObj()
            {
                LogSymbol = LogSymbol.TwilioToWeb,
                CallInstance = ci,
                Caption = "Call End Status to " + ci.CallBackurl+ " "+Reason 
            };


            try
            {
                v.AddNode("Request", nvc);
                 a = ci.BrowserClient.DoRequest(ci.CallOptions.StatusCallback, nvc);
                v.AddNode("Response", a);
                
            }
            catch (Exception ex)
            {
                v.Caption = "<- Exception on Call Start Call Back to " + ci.CallBackurl+ex.Message ;
            }
            SystemController.Instance.Logger.LogObj(v);
            ci.CallStatus = CallStatus.Ended;
            return a;
        } 
        
        
        #region Search Phone List
        public CallInstance GetCallInstanceFromPhoneNumber(string phonenumber)
        {
            var a = CallList.Where(x => (x.CallForGet.To == phonenumber && x.CallStatus != CallStatus.Ended)).First();
            return a;
        }

        public CallInstance GetCallInstanceFromCallSid(string sid)
        {
            sid = sid.ToUpper();
            var a = CallList.Where(x => (x.CallForGet.Sid.ToUpper()==sid)).FirstOrDefault();
            return a;
        }
#endregion

       
    }

   

}
