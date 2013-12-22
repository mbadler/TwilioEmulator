using Twilio;
using System.Collections.Generic;
using System.Net;
using System.Drawing;
using System;

namespace TwilioEmulator.Code
{
    public class CallInstance : object
    {
        Call Call { get; set; }
        public Call CallForGet
        {
            get
            {
                return Call;
            }
            
        }

       public Call CallForSet
        {
            get
            {
                Call.DateUpdated = DateTime.Now;
                return Call;
            }
          
        }

       public Call CallForCreate
       {
           get
           {
               return Call;
           }
           set
           {
               Call = value;
           }
       }
        
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
            FlowEngine = new CallFlowEngine() { MyCall = this };
        }

        public CallFlowEngine FlowEngine = null;

        public string CallBackurl { get; set; }

     

        public void StartCallFlow()
        {
            FlowEngine.StartCallFlow();
        }
        //public DateTime LastAction { get; set; }

        internal void AbortCallFlow()
        {
            if (FlowEngine.thread != null)
            {
                FlowEngine.thread.Abort();
            }
        }
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
    /// Call is all connected ane we are ready to start the flow engine
    /// </summary>
    ReadyForProcessing,
    /// <summary>
    /// we sent a callback to the server  - don't process
    /// </summary>
    CallbackSent,
    /// <summary>
    ///  Call Flow is processing
    /// </summary>
    ProcessingCall,
    Ended
    
}
}