using Twilio;
using TwilioEmulator.API.Infrastructure;
using TwilioEmulator.Code;
using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;

namespace TwilioEmulator.API
{
    public class CallsController : BaseController
    {
        public Call Post(CallOptions c)
        {
          
            

            var cl = SystemController.Instance.Office.NewCallRequest(c);
            
            var lg = new LogObj()
            {
                Caption = "Call request from application ",
                LogSymbol = Code.LogSymbol.WebToTwilio,
                CallInstance = cl
            };

            lg.AddNode("Request", c).AddNode("Response", cl.CallForGet).LogIt();
             
            return cl.CallForGet;
        }

        [HttpGet]
        public Call Modify([FromUri]string id )
        {
            var d = SystemController.Instance.Office.GetCallInstanceFromCallSid(id);
            var lg = new LogObj()
            {
                Caption = "Call status request ",
                LogSymbol = Code.LogSymbol.WebToTwilio,
                CallInstance = d
            };

            lg.AddNode("Request", d).AddNode("Response", d.CallForGet).LogIt();
             
           return d.CallForGet;
        }

         [HttpPost]
        public Call Modify([FromUri]string id, HttpRequestMessage request)
        {
            
            var a = request.RequestUri.ParseQueryString();
             var d = SystemController.Instance.Office.GetCallInstanceFromCallSid(id);
             var stat = a.Get("Status");
             if (stat != "")
             {

                 var lg = new LogObj()
                 {
                     Caption = "Calls Hang up request - "+stat,
                     LogSymbol = Code.LogSymbol.WebToTwilio,
                     CallInstance = d
                 }.AddNode("Request",request);

                 SystemController.Instance.Office.MarkCallEnded(d, stat,true);
                 lg.AddNode("Response", d).LogIt();
                 
             }
            return d.CallForGet;
        }


         [HttpPost]
        public Call GetStatus([FromUri]string id, HttpRequestMessage request)
        {
            //var a = request.RequestUri.ParseQueryString();
            //a.Get(
            return null;
        }
    }
}
