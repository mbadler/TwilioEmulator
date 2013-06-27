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
            MyCall.CallStatus = CallStatus.ProcessingCall;
            Task.Factory.StartNew(() =>
                {

                    TwimlPath = MyCall.CallOptions.Url;
                    TwimlLogAs = "Inital Call";
                    // get the introductory twiml
                    // start procesing
                    while (MyCall.CallStatus != CallStatus.Ended)
                    {
                        DoTwimlRequest(TwimlPath ,TwimlLogAs);
                        ProcessReceivedTwiml();
                    }
                    

                }
                 , TaskCreationOptions.LongRunning);
        }

        private void ProcessReceivedTwiml()
        {
            foreach (var node in TwimlVerbs())
            {
                // find the right delegate and execute it
                VerbFunctions.Where(x => x.Method.Name.ToUpper() == node.Name.ToString().ToUpper()).First()(node);
            }
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
                v.AddNode("TWiml Error","Error parsing Twiml "+ex.Message+ " Defaulting to a 5 seconds pause").IsInError = true;
                Twiml = XDocument.Parse(@"<?xml version=""1.0"" encoding=""UTF-8"" ?><Response><Pause length=""10""/></Response>");
            }
            SystemController.Instance.Logger.LogObj(v);


        }

        #region Twiml Verb Functions
        protected void Pause(XElement twimnode)
        {
            var len = int.Parse(twimnode.Attribute("length").Value);

            Thread.Sleep(len * 1000);
        }
        #endregion
    }

    public delegate void TwimlVerbFunction(XElement twimnode);
}
