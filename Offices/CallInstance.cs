using Twilio;
using System.Collections.Generic;
using System.Net;
using System.Drawing;
using System;

namespace TwilioEmulator.Code
{
    public class CallInstance : object
    {
        public Call Call { get; set; }
        public CallDirection CallDirection { get; set; } 

        public CallStatus CallStatus { get; set; }
        public CallOptions CallOptions { get; set; }

        public string Digits { get; set; }

        public CallStatusWebClient BrowserClient = new CallStatusWebClient();

        public Color CallColor;

        public bool IsNewCall { get; set; }

        public CallInstance()
        {
            IsNewCall = true;
            CallStatus = Code.CallStatus.Queued;
            Random rand = new Random();
            CallColor = Color.FromArgb(rand.Next(200,256), rand.Next(200,256), rand.Next(200,256));
        }

        public string LatestTwiml { get; set; }

        public string CallBackurl { get; set; }

        //public DateTime LastAction { get; set; }
    }

public enum CallDirection 
{
    In,Out
}

    /// <summary>
    /// The lifecyctle of the processing of this call
    /// </summary>
public enum CallStatus
{
    /// <summary>
    /// Waiting to be dialed - process
    /// </summary>
    Queued, 
    /// <summary>
    /// Waitng for phone to answer - process for timeout
    /// </summary>
    WaitingRinging,
    /// <summary>
    /// Don't Process - we are cusrrently dealing iwith things
    /// </summary>
    LocalProcessing,
    /// <summary>
    /// we sent a callback to the server  - don't process
    /// </summary>
    CallbackSent,
    /// <summary>
    ///  we got some teiml or something - process
    /// </summary>
    ProcessingCall,
    Ended
    
}
}