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

        public XElement GatherNode = null;
        public XElement CurrentNode = null;

        public Thread thread = null;

        public List<TwimlVerbFunction> VerbFunctions = new List<TwimlVerbFunction>();

        public IEnumerable<XElement> TwimlVerbs()
        {
            while (MyCall.CallStatus != CallStatus.Ended)
            {
                if (CurrentNode == null)
                {
                    var allNodes = Twiml.Element("Response").Elements();
                    if (allNodes.Count() == 0)
                    {
                        // no nodes at all!!!!!
                        yield break;
                    }
                    CurrentNode = allNodes.First();

                }
                else
                {
                    if (CurrentNode.NextElement() == null)
                    {

                        var parnode = CurrentNode.Parent;
                        // it either a gather or the end of the docuemtn
                        // if its teh end of the document then exit
                        if (parnode.Name.ToString().ToUpper() == "RESPONSE")
                        {
                            // exit the loop
                            yield break;
                        }
                        else
                        {
                            // its the gather verb
                            // we have played all of the children verbs so now set the gather so that we cna begin the final timeout
                            CurrentNode = GatherNode;
                            // the gather might be the last in the dcouemnt
                            
                        }
                    }
                    else
                    {
                        // ok - so seems that there is a sister node
                        CurrentNode = CurrentNode.NextElement();
                    };

                };

                // if its the gather verb then set that as the gethernode and lift the current emenet to the gather child
                // unless we are the gather verb then just continue
                if (GatherNode == null && CurrentNode.Name.ToString().ToUpper() == "GATHER")
                {
                    // its teh gather verb
                    // set it as the gathernode
                    GatherNode = CurrentNode;
                    // if there are children(play etc...,) then set them as the current node
                    // else keep it to the gather
                    var decs = CurrentNode.Descendants();
                    if (decs.Count() > 0)
                    {
                        CurrentNode = decs.First();
                    }

                }

                yield return CurrentNode;
            }

        }

        void StartGather(XElement GatherNode)
        {

            this.GatherNode = GatherNode;
     
        }

        internal void StartCallFlow()
        {
            // start as a new thread
            PopulateVerbFunctions();

            MyCall.CallStatus = CallStatus.ProcessingCall;
            Task.Factory.StartNew(() =>
                {
                    thread = Thread.CurrentThread;
                    TwimlPath = MyCall.CallOptions.Url;
                    TwimlLogAs = "Initial Call";
                    // get the introductory twiml
                    // start procesing
                    TwimlVerbResult tvr = TwimlVerbResult.Redirect;
                    while (MyCall.CallStatus != CallStatus.Ended  && tvr == TwimlVerbResult.Redirect)
                    {
                        DoTwimlRequest(TwimlPath ,TwimlLogAs);
                        MyCall.Digits = "";
                        GatherNode = null;
                        CurrentNode = null;
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
                v.AddException(ex);

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

        private void PopulateVerbFunctions()
        {
            VerbFunctions.Add(Pause);
            VerbFunctions.Add(Hangup);
            VerbFunctions.Add(Redirect);
            VerbFunctions.Add(Gather);
            VerbFunctions.Add(Say);
            VerbFunctions.Add(Play);
            VerbFunctions.Add(SMS);
        }

        protected TwimlVerbResult SMS(XElement twimnode)
        {
            return SendToPhone(twimnode);

        }

        protected TwimlVerbResult Play(XElement twimnode)
        {
            return SendToPhone(twimnode);

        }

        private TwimlVerbResult SendToPhone(XElement twimnode)
        {
            var words = twimnode.Value;
            var verb = twimnode.Name;
            var a = new LogObj()
            {
                Caption = "Twiml "+verb+": " + words,
                LogSymbol = LogSymbol.TwilioToPhone,
                CallInstance = MyCall
            }.LogIt();



            SystemController.Instance.Office.SayToPhone(MyCall, verb+"ing (" + words + ")");


            return TwimlVerbResult.Continue;
        }

        protected TwimlVerbResult Say(XElement twimnode)
        {
             var words = twimnode.Value;
             var a = new LogObj()
             {
                 Caption = "Twiml Say: "+words,
                 LogSymbol = LogSymbol.TwilioToPhone,
                 CallInstance = MyCall
             }.LogIt();

            
            foreach (var wd in twimnode.Value.Split(' '))
            {
                SystemController.Instance.Office.SayToPhone(MyCall, wd);
                Thread.Sleep(200);
            }
            
            return TwimlVerbResult.Continue;

        }

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

        protected TwimlVerbResult Gather(XElement twimnode)
        {
            return TwimlVerbResult.Continue;
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
