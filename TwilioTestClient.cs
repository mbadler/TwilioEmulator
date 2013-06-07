using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;
using RestSharp;
using System.Net;
using System.Reflection;

namespace TwilioEmulator
{
    public class TwilioTestClient
    {
        public static TwilioRestClient GetTestClient(string accountSid, string authToken)
        {
            var a = new TwilioRestClient(accountSid, authToken);
            Type type = a.GetType();
            var b = (RestClient)type.GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(a);
            b.BaseUrl = "http://Localhost:18080/2010-04-01";
            b.Proxy = new WebProxy("Localhost", 8888);

            return a;
        }
    }
}
