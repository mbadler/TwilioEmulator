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
                x.CallStatus==CallStatus.ProcessingCall||
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
               
                ProcessCallPhone(ci);
            }

            if (ci.CallStatus == CallStatus.ProcessingCall)
            {
                // this is a first time request - goto the place specified in the url parameter
               // run some twiml

            }
            
        }

        private void ProcessDoTwimlRequest(CallInstance ci,string UrlToUse,string LogAs)
        {
            ci.CallStatus = CallStatus.CallbackSent;
            var a = "";
            var nvc = ci.GenerateCallBackValue();
            try
            {
                a = ci.BrowserClient.DoRequest(UrlToUse,nvc);
                SystemController.Instance.Logger.Log2Nodes("() <-- []   "+LogAs+" to "+ UrlToUse, "Request", nvc, "Response", a, false);
                ci.LatestTwiml = a;
            }
            catch (Exception ex)
            {
                SystemController.Instance.Logger.Log2Nodes("<- Exception on Call Start Call Back to " + ci.CallBackurl + ex.Message, "Request", nvc, "Response", a, false);
            }
            ci.CallStatus = CallStatus.ProcessingCall;

        }

        private void ProcessCallPhone(CallInstance ci)
        {
            // when we go to multi phones we will look up by phone number etc... - but for now 
            // we will look at the first phone
            // if there is no first phone then fire the need phone eventhandler
            
            Boolean Callok = false;

            var c = ci.Call;    
            switch (SystemController.Instance.PhoneManager.AttemptDial(ci.Call.To))
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
                SendCallEndInErrorCallBack(ci, ci.GenerateCallBackValue(),c.Status);
            }
            else
            {
                ci.CallStatus = CallStatus.WaitingRinging;
            }
            
        }

        public void PhonePickedUp(string phonenumber,bool IsMachine)
        {
            // find the ci with that phone number
            SystemController.Instance.Logger.LogLine("[] <--  }   Phone Picked up "+phonenumber+(IsMachine?" Machine":"") );
            var a = CallList.Where(x => (x.Call.To == phonenumber && x.CallStatus != CallStatus.Ended)).First();
            if (a.CallOptions.IfMachine != "")
            {
                if (!IsMachine)
                {
                    a.Call.AnsweredBy = "Human";
                }
                else
                {
                    a.Call.AnsweredBy= "Machine";
                    if (a.CallOptions.IfMachine == "Hangup")
                    {
                        HangUpThePhoneConnectionFromTheserver(a, "Answered by machine");
                        SendCallEndInErrorCallBack(a, a.GenerateCallBackValue(),"Answered by answering machine");
                        a.CallStatus = CallStatus.Ended;
                        return;
                    }
                    
                }
            }
               
            a.Call.Status = TwilioCallStatuses.INPROGRESS;
            ProcessDoTwimlRequest(a, a.CallOptions.Url,"Get Welcome message Twiml");

        }

        private void HangUpThePhoneConnectionFromTheserver(CallInstance ci,string Reason)
        {

            SystemController.Instance.Logger.LogLine("[] -->  }   Call Hung up " + ci.CallOptions.To +  " "+Reason);
            SystemController.Instance.PhoneManager.CallHungUp(ci.CallOptions.To, Reason);

        }


        protected string SendCallEndInErrorCallBack(CallInstance ci, NameValueCollection nvc,string Reason)
        {
            ci.CallStatus = CallStatus.CallbackSent;
            var a = "";
            
            try
            {
                 a = ci.BrowserClient.DoRequest(ci.CallOptions.StatusCallback, nvc);
                SystemController.Instance.Logger.Log2Nodes("() <-- []   Call Connect Error start to " + ci.CallBackurl+ " "+Reason, "Request", nvc, "Response", a, false);
            }
            catch (Exception ex)
            {
                SystemController.Instance.Logger.Log2Nodes("<- Exception on Call Start Call Back to " + ci.CallBackurl+ex.Message, "Request", nvc, "Response", a, false);
            }
            ci.CallStatus = CallStatus.Ended;
            return a;
        }
    }

   
}
