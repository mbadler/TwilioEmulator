using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TwilioEmulator.Code.Extensions;
using System.Threading.Tasks;
using System.Threading;
using TwilioEmulator.Code.Models;

namespace TwilioEmulator.Code
{
    public class CallFlowEngine
    {
        public XDocument Twiml { get; set; }
        public CallInstance MyCall { get; set; }


        public string _twimlPath = "";
        public string TwimlPath
        {
            get
            {
                return _twimlPath;
            }
            set
            {
                if (value == "" || _twimlPath == "")
                {
                    _twimlPath = value;
                    return;
                }

                _twimlPath = new Uri(new Uri(_twimlPath), value).AbsoluteUri;
            }
        }
        public string TwimlLogAs = "";

        public XElement GatherNode = null;
        public XElement CurrentNode = null;

        public string CurrentUrl = "";

        public Thread thread = null;

        public List<TwimlVerbFunction> VerbFunctions = new List<TwimlVerbFunction>();

        #region Verb Loop Processing


       

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
                    // we need to process siblings
                    if (CurrentNode.NextElement() == null)
                    {
                        // no siblings
                        // set it to the parent node
                        // if we currenttl are the gather node then zero it out first
                        
                        CurrentNode = CurrentNode.Parent;
                    }
                    else
                    {
                        //set it to tthe next sibling
                        if (GatherNode == CurrentNode)
                        {
                            GatherNode = null;
                        }
                        CurrentNode = CurrentNode.NextElement();
                        // Set the parent node as the active one
                    }

                };
                if (CurrentNode.Name.ToString().ToUpper() == "RESPONSE")
                {
                    // we are done
                    yield break;
                }
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
                while (MyCall.CallStatus != CallStatus.Ended && tvr == TwimlVerbResult.Redirect)
                {
                    DoTwimlRequest(TwimlPath, TwimlLogAs);
                    MyCall.Digits = "";
                    GatherNode = null;
                    CurrentNode = null;
                    

                    tvr = ProcessReceivedTwiml();
                }

                SystemController.Instance.Office.MarkCallEnded(MyCall, "Twiml Execution ended", true);


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

                var tvr =  RunTwimlVerb(node);
                if (tvr != TwimlVerbResult.Continue)
                {
                    return tvr;
                }

            }
            return TwimlVerbResult.Continue;

        }

        private TwimlVerbResult RunTwimlVerb(XElement node)
        {
            var verbfunc = VerbFunctions.Where(x => x.Method.Name.ToUpper() == node.Name.ToString().ToUpper()).First();
            TwimlVerbResult tvr = TwimlVerbResult.Continue;
            Thread ExecutingThread = null;
            var status = Task.Factory.StartNew(() =>
            {
                ExecutingThread = Thread.CurrentThread;
                tvr = verbfunc(node);

            });
            while (!status.IsCompleted)
            {
                Thread.Sleep(100);
                if (GatherNode != null && GatherNode != CurrentNode) 
                {
                    
                    if (MyCall.Digits != "")
                    {
                        ExecutingThread.Abort();
                        CurrentNode = GatherNode;
                        return RunTwimlVerb(GatherNode);
                    }
                }
            }

            
            return tvr;
           
        }

        #endregion
        


#region Gather Specific Processing
        void StartGather(XElement GatherNode)
        {

            this.GatherNode = GatherNode;
            MyCall.Digits = "";
     
        }

        
#endregion




        private void DoTwimlRequest( string UrlToUse, string LogAs)
        {
            CurrentUrl = UrlToUse;
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
                v.AddNode("Twiml:",a);
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
            VerbFunctions.Add(Reject);
        }

        protected TwimlVerbResult SMS(XElement twimnode)
        {
            string action = twimnode.Attributes("action").Any() ?
                twimnode.Attribute("timeout").Value : "";


            SMSOptions s = new SMSOptions()
            {
                to = twimnode.Attribute("to").Value,
                from = twimnode.Attribute("from").Value,
                body = twimnode.Attribute("body").Value,
                statusCallback = twimnode.Attribute("statusCallback").Value
            };
            SystemController.Instance.Office.NewSMSRequest(s);

            if (action != "")
            {
                TwimlPath = action;
                TwimlLogAs = "SMS Action URL ";
                return TwimlVerbResult.Redirect;
            }
            else
            {
                return TwimlVerbResult.Continue;
            }

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
            Thread.Sleep(len * 1000);
            return TwimlVerbResult.Continue;
            
        }



        protected TwimlVerbResult Hangup(XElement twimnode)
        {
            SystemController.Instance.Office.MarkCallEnded(this.MyCall, "FromTwiml",true);
            return TwimlVerbResult.EndCall;

        }

        protected TwimlVerbResult Reject(XElement twimnode)
        {
            SystemController.Instance.Office.MarkCallEnded(this.MyCall,"TwimlReject", true);
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
            var timeout = twimnode.Attributes("timeout").Any() ?
                int.Parse(twimnode.Attribute("timeout").Value) * 2 : 10;

            var finishOnKey = twimnode.Attributes("finishOnKey").Any() ?
                twimnode.Attribute("finishOnKey").Value : "#";

            var numDigits = twimnode.Attributes("numDigits").Any() ?
                int.Parse(twimnode.Attribute("numDigits").Value) : int.MaxValue;

            var action = twimnode.Attributes("action").Any() ?
                twimnode.Attribute("action").Value : TwimlPath;

            TwimlLogAs = "Gather Submit Action";


            for (int i = 0; i < timeout; i++)
            {
                var dig = MyCall.Digits;
                if (dig.Contains(finishOnKey) || dig.Length >= numDigits)
                {
                    break;
                }


                Thread.Sleep(500);
            }


            if (MyCall.Digits.EndsWith(finishOnKey)) 
            {
                MyCall.Digits = MyCall.Digits.Substring(0,MyCall.Digits.Count()-1);
            }

            if (MyCall.Digits.Count() > numDigits)
            {
                MyCall.Digits = MyCall.Digits.Substring(0, numDigits);
            }

            if (MyCall.Digits != "")
            {
                TwimlPath = action;
            }



            return MyCall.Digits == "" ? TwimlVerbResult.Continue : TwimlVerbResult.Redirect;
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
