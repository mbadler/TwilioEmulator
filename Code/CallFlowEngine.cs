using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TwilioEmulator.Code.Extensions;
using System.Threading.Tasks;
using System.Threading;

namespace TwilioEmulator.Code
{
    public class CallFlowEngine
    {
        public XDocument Twiml { get; set; }
        public CallInstance MyCall { get; set; }

        public string TwimlPath = "";
        public string TwimlLogAs = "";

        public List<TwimlVerbFunction> VerbFunctions = new List<TwimlVerbFunction>();

        public IEnumerable<XElement> TwimlVerbs()
        {
            foreach (var node in Twiml.Element("Response").Elements())
            {
                yield return node;
            }
        }


        internal void StartCallFlow()
        {
            // start as a new thread
            VerbFunctions.Add(Pause);
            VerbFunctions.Add(Hangup);
            VerbFunctions.Add(Redirect);

            MyCall.CallStatus = CallStatus.ProcessingCall;
            Task.Factory.StartNew(() =>
                {

                    TwimlPath = MyCall.CallOptions.Url;
                    TwimlLogAs = "Initial Call";
                    // get the introductory twiml
                    // start procesing
                    TwimlVerbResult tvr = TwimlVerbResult.Redirect;
                    while (MyCall.CallStatus != CallStatus.Ended  && tvr == TwimlVerbResult.Redirect)
                    {
                        DoTwimlRequest(TwimlPath ,TwimlLogAs);
                        TwimlPath = "";
                        tvr = ProcessReceivedTwiml();
                    }

                    SystemController.Instance.Office.MarkCallEnded(MyCall,"Twiml Execution ended",true);
                    

                }
                 , TaskCreationOptions.LongRunning);
        }

        private TwimlVerbResult ProcessReceivedTwiml()
        {
            var curl = this.TwimlPath;
            foreach (var node in TwimlVerbs())
            {
                // Mark what the current url should be - if that changes then pop out and let the rerequset happen
                
                // find the right delegate and execute it
                
                 TwimlVerbResult tvr =   VerbFunctions.Where(x => x.Method.Name.ToUpper() == node.Name.ToString().ToUpper()).First()(node);
                 if (tvr != TwimlVerbResult.Continue)
                 {
                     return tvr;
                 }

                
            }
            return TwimlVerbResult.Continue;

        }
        


        private void DoTwimlRequest( string UrlToUse, string LogAs)
        {
            
            var a = "";
            var nvc = MyCall.GenerateCallBackValue();
            var v = new LogObj()
            {
                LogSymbol = LogSymbol.TwilioToWeb,
                CallInstance = MyCall,
                Caption = LogAs + " to " + UrlToUse
            };



            try
            {
                v.AddNode("Request", nvc);
                a = MyCall.BrowserClient.DoRequest(UrlToUse, nvc);
                v.AddNode("Response", a);


            }
            catch (Exception ex)
            {
                v.Caption = "<- Exception on Call Start Call Back to " + MyCall.CallBackurl + ex.Message;

            }
            try
            {
                Twiml = XDocument.Parse(a);
                v.AddNode("Twiml:",Twiml.Nodes().Count().ToString()+ " Nodes");
            }
            catch (Exception ex)
            {
                v.AddNode("TWiml Error","Error parsing Twiml "+ex.Message+ " Defaulting to a blank document").IsInError = true;
                Twiml = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF-8"" ?><Response></Response>");
            }
            SystemController.Instance.Logger.LogObj(v);


        }

        #region Twiml Verb Functions
        protected TwimlVerbResult Pause(XElement twimnode)
        {
            var len = int.Parse(twimnode.Attribute("length").Value);
            for (int i = 0; i <= len*2; i++)
            {
                // we need to keep it responsive in case the call is shut down
                if (MyCall.CallStatus == CallStatus.Ended)
                {
                    return TwimlVerbResult.EndCall;
                }
                Thread.Sleep(500);

            }
            return TwimlVerbResult.Continue;
            
        }

        protected TwimlVerbResult Hangup(XElement twimnode)
        {
            SystemController.Instance.Office.MarkCallEnded(this.MyCall, "FromTwiml",true);
            return TwimlVerbResult.EndCall;

        }

        protected TwimlVerbResult Redirect(XElement twimnode)
        {
            TwimlPath = twimnode.Value;
            TwimlLogAs = "Twiml Redirect verb ";
            return TwimlVerbResult.Redirect;

        }

        #endregion
    }

    public delegate TwimlVerbResult TwimlVerbFunction(XElement twimnode);


}

public enum TwimlVerbResult
{
    Continue,
    EndCall,
    Redirect
}
