using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwilioEmulator.API.Infrastructure;
using System.Web.Http;
using TwilioEmulator.Code.Models;
using TwilioEmulator.Code;
using Twilio;

namespace TwilioEmulator.API
{
    public class SMSController : BaseController
    {
        public string Get(string index)
        {
            return "index";
        }

        [HttpPost]
        public SMSMessage Messages(SMSOptions options)
        {
            var a = SystemController.Instance.Office.NewSMSRequest(options);
            var lg = new LogObj()
            {
                Caption = "SMS Request From Application ",
                LogSymbol = Code.LogSymbol.WebToTwilio,
                
            };

            lg.AddNode("Request", options).AddNode("Response", a).LogIt();
            return a;
        }
    }
}
