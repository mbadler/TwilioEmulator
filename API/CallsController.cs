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
            

            var cl = SystemController.Instance.Office.NewCallRequest(c);
            
            var lg = new LogObj()
            {
                Caption = "Call request from application ",
                LogSymbol = Code.LogSymbol.WebToTwilio,
                CallInstance = cl
            };

            lg.AddNode("Request", c).AddNode("Response", cl.Call);
            SystemController.Instance.Logger.LogObj(lg);
            return cl.Call;
        }
    }
}
