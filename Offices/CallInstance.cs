using Twilio;
using System.Collections.Generic;
using System.Net;

namespace TwilioEmulator.Code
{
    public class CallInstance : object
    {
        public Call Call { get; set; }
        public CallDirection CallDirection { get; set; } 
        public CookieContainer Cookies {get;set;}
        public CallStatus CallStatus { get; set; }


        public CallInstance()
        {
            Cookies = new CookieContainer();
            CallStatus = Code.CallStatus.Queued;
        }


        public string CallBackurl { get; set; }
    }

public enum CallDirection 
{
    In,Out
}

public enum CallStatus
{
    Queued,
    CallbackSent,
    WaitingForPhone,
    Ended
}
}