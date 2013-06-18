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
        public static NameValueCollection GenerateCallBackValue(this Call c)
        {
            return new NameValueCollection()
            {
                {"AccountSid",c.AccountSid},
                {"CallSid",c.Sid},
                {"CallStatus",c.Status},
                {"From",c.From},
                {"To",c.To}
            };
            

        }
    }
}
