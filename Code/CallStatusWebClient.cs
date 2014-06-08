using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Security.Cryptography;
using TwilioEmulator.Properties;

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
            // create the keys
            var value = new StringBuilder();


            value.Append(url);

            // If the request is a POST, take all of the POST parameters and sort them alphabetically.
           
            {
                // Iterate through that sorted list of POST parameters, and append the variable name and value (with no delimiters) to the end of the URL string
                var sortedKeys = nvc.AllKeys.OrderBy(k => k, StringComparer.Ordinal).ToList();
                foreach (var key in sortedKeys)
                {
                    value.Append(key);
                    value.Append(nvc[key]);
                }
            }

            // Sign the resulting value with HMAC-SHA1 using your AuthToken as the key (remember, your AuthToken's case matters!).
            var sha1 = new HMACSHA1(Encoding.UTF8.GetBytes(Settings.Default.AuthToken));
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(value.ToString()));

            // Base64 encode the hash
            var encoded = Convert.ToBase64String(hash);
            this.Headers["X-Twilio-Signature"] = encoded;
            return Encoding.ASCII.GetString(UploadValues(url, nvc));
        }
    }
}

