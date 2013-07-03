using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;

namespace TwilioEmulator.Code
{
    public class CallStatusWebClient : WebClient
    {

        public CookieContainer Cookies = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.KeepAlive = false;
            if (webRequest != null)
            {
                webRequest.CookieContainer = Cookies;
            }
            return request;
        }

        public string DoRequest(string url, NameValueCollection nvc)
        {
            return Encoding.ASCII.GetString(UploadValues(url, nvc));
        }
    }
}

