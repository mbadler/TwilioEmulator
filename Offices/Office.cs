using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwilioEmulator.Code;
using TwilioEmulator.Phones;
using Twilio;

namespace TwilioEmulator.Offices
{
    public class Office:OfficeBase
    {


        public List<CallInstance> CallList = new List<CallInstance>();
        public Dictionary<string,PhoneBase> PhoneList = new Dictionary<string,PhoneBase>();

        public CallInstance NewCallRequest(CallOptions co)
        {
            CallInstance c = new CallInstance()
            {
                Call = new Call()
                {
                    Sid = Guid.NewGuid().ToString().Replace("-", ""),
                    Status = "queued",
                    Direction = "outbound-api",
                    DateCreated = DateTime.Now.ToUniversalTime(),
                    DateUpdated = DateTime.Now.ToUniversalTime(),
                    To = co.To,
                    From = co.From
                },
                CallDirection = CallDirection.Out
            };

            CallList.Add(c);
            return c;
        }

        public override void Process()
        {
            // run thru each call and see what needs to be done with them
            CallList.AsParallel().ForAll(x=>
                {
                    ProcessCallInstance(x);
                });
            
        }

        void ProcessCallInstance(CallInstance ci)
        {

        }
    }
}
