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
            var v = new NameValueCollection()
            {
                {"AccountSid",c.Call.AccountSid},
                {"CallSid",c.Call.Sid},
                {"CallStatus",c.Call.Status},
                {"From",c.Call.From},
                {"To",c.Call.To}
            };
            if (c.Digits != "")
            {
                v.Add("Digits", c.Digits);
            }
            if (c.Call.AnsweredBy != "")
            {
                v.Add("AnsweredBy", c.Call.AnsweredBy);
            }
            return v;

        }
    }
}
