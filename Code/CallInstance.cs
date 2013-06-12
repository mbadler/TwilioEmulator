using Twilio;
using System.Collections.Generic;

namespace TwilioEmulator.Code
{
    public class CallInstance : object
    {
        public Call Call { get; set; }
        public CallDirection CallDirection { get; set; } 
    }
}
public enum CallDirection 
{
    In,Out
}