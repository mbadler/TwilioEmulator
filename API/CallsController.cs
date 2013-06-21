using Twilio;
using TwilioEmulator.API.Infrastructure;
using TwilioEmulator.Code;
using System.Collections.Generic;

namespace TwilioEmulator.API
{
    public class CallsController : BaseController
    {
        public Call Post(CallOptions c)
        {
            var a = new Dictionary<string, object>();
            a.Add("Request",c);

            var cl = SystemController.Office.NewCallRequest(c);
            a.Add("Response",cl.Call);
            SystemController.Instance.Logger.LogDictionaryOfObjects("() --> []  CallRequest",a);
            return cl.Call;
        }
    }
}
