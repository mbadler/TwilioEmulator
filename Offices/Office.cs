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


        public CallInstance NewCallRequest(CallOptions co)
        {
            CallInstance c = new CallInstance()
            {
                Call = new Call()
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

            var c = ci.Call;
            var v = SystemController.Instance.PhoneManager.AttemptDial(ci.Call.To);
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
                        c.Status = TwilioCallStatuses.BUSY;

                        break;
                    }
                case PhoneStatus.Ringing:
                    {
                        Callok = true;
                        c.Status = TwilioCallStatuses.RINGING;
                        break;
                    }
                case PhoneStatus.NotInService:
                    {
                        c.Status = TwilioCallStatuses.FAILED;
                        break;
                    }
                case PhoneStatus.NoAnswer:
                    {
                        c.Status = TwilioCallStatuses.NOANSWER;
                        break;
                    }
                default:
                    break;
            }
            // if it is not a good call send back a error call status and mark the call done
            // else mark it as waiting for the phone
            if (!Callok)
            {
                ci.CallStatus = CallStatus.Ended;
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
            a.Call.Status = TwilioCallStatuses.COMPLETED;
            SendCallEndStatus(a, a.GenerateCallBackValue(), "Hung up");
            a.CallStatus = CallStatus.Ended;
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
                    a.Call.AnsweredBy = "Human";
                }
                else
                {
                    a.Call.AnsweredBy = "Machine";
                    if (a.CallOptions.IfMachine == "Hangup")
                    {
                        HangUpThePhoneConnectionFromTheserver(a, "Answered by machine");
                        SendCallEndStatus(a, a.GenerateCallBackValue(), "Answered by answering machine");
                        a.CallStatus = CallStatus.Ended;
                        return;
                    }

                }
            }


            a.CallStatus = CallStatus.ReadyForProcessing;
            //ProcessDoTwimlRequest(a, a.CallOptions.Url, "Get Welcome message Twiml");

        }

        public CallInstance GetCallInstanceFromPhoneNumber(string phonenumber)
        {
            var a = CallList.Where(x => (x.Call.To == phonenumber && x.CallStatus != CallStatus.Ended)).First();
            return a;
        }

        private void HangUpThePhoneConnectionFromTheserver(CallInstance ci,string Reason)
        {
            var v = new LogObj()
            {
                Caption = "X Server hung up call " + Reason,
                LogSymbol = Code.LogSymbol.TwilioToPhone,
                CallInstance = ci
            };


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
    }

   
}
