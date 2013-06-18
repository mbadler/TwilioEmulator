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
                    From = co.From,
                    
                },
                CallDirection = CallDirection.Out,
                CallBackurl = co.StatusCallback
               
            };

            CallList.Add(c);
            return c;
        }

        protected override void Process()
        {
            // run thru each call and see what needs to be done with them
            CallList.AsParallel().ForAll(x=>
                {
                    ProcessCallInstance(x);
                });
            
        }

        void ProcessCallInstance(CallInstance ci)
        {
            if (ci.CallStatus == CallStatus.Queued && ci.CallDirection == CallDirection.Out)
            {
                ci.CallStatus = CallStatus.CallbackSent;
                ProcessCallPhone(ci);
            }
            
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
            var twiml = SendCallCallBack(ci, ci.Call.GenerateCallBackValue());
        }


        protected string SendCallCallBack(CallInstance ci,NameValueCollection nvc)
        {
            string a = "";
            CallStatusWebClient wc = new CallStatusWebClient();
            try
            {
                 a = Encoding.ASCII.GetString(wc.UploadValues(ci.CallBackurl, nvc));
            }
            catch (Exception ex)
            {
                SystemController.Instance.Logger.Log2Nodes("<- Exception on Call Start Call Back to " + ci.CallBackurl+ex.Message, "Request", nvc, "Response", a, false);
            }
            SystemController.Instance.Logger.Log2Nodes("<- Call Start Call Back to " + ci.CallBackurl, "Request", nvc, "Response", a, false);
            return a;
        }
    }

   
}
