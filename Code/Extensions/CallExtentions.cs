using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;
using System.Collections.Specialized;

namespace TwilioEmulator.Code.Extensions
{
    public static class CallExtentions
    {
        public static NameValueCollection GenerateCallBackValue(this CallInstance c)
        {
            var call = c.CallForGet;
            var v = new NameValueCollection()
            {
                {"AccountSid",call.AccountSid},
                {"CallSid",call.Sid},
                {"CallStatus",call.Status},
                {"From",call.From},
                {"To",call.To},
                {"Direction",call.Direction}
            };
            if (c.Digits != "")
            {
                v.Add("Digits", c.Digits);
            }
            if (call.AnsweredBy != "")
            {
                v.Add("AnsweredBy", call.AnsweredBy);
            }
            return v;

            

        }
    }
}
