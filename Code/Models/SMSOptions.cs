using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwilioEmulator.Code.Models
{
    public class SMSOptions
    {
        public string from {get;set;}
        public string to {get;set;}
        public string body {get;set;}
        public string statusCallback {get;set;}
        public string applicationSid {get;set;}
         
    }
}
