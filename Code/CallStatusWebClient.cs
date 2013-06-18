using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwilioEmulator.Code
{
    public class CallStatusWebClient : WebClient
    {

        public CookieContainer Cookies { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = Cookies;
            }
            return request;
        }
    }
}

