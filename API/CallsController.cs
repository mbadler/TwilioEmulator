using Twilio;
using TwilioEmulator.API.Infrastructure;
using TwilioEmulator.Code;

namespace TwilioEmulator.API
{
    public class CallsController : BaseController
    {
        public Call Post(CallOptions c)
        {
            SystemController.Instance.LogObjectDump((object)c, "Call");
            var cl = SystemController.Instance.CreateNewInboudCall(c);
            return cl;
        }
    }
}
