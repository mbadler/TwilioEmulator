using Twilio;
using System.Collections.Generic;

namespace TwilioEmulator.Code
{
    public class CallInstance : Call
    {
        public Dictionary<string, string> subresource_uris { get; set; }
    }
}
