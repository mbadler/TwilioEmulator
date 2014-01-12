using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwilioEmulator.Phones;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace TwilioEmulator.Code.ScriptEngines
{
    public class ScriptRunner:IPhoneManager
    {
        public string buffer = "";
        public List<ParsedTelScript> ParsedScripts = null;

        public string ScriptText { get; set; }

        public string PhoneNumber { get; set; }

        public bool isWaiting = false;

        public bool ShouldCancel { get; set; }

        public void Run()
        {
            Task.Factory.StartNew(() =>
                {
                    foreach (var x in ParsedScripts)
                    {

                        DateTime? WaitUntil = null;

                        if (ShouldCancel)
                        {
                            return;
                        }
                        if (x.Command.ToUpper() == "DIGITS")
                        {

                            SystemController.Instance.Office.PhoneSendingDigits(PhoneNumber, x.Text.Trim());
                            CallLogger.LogDigitPressed(x.Text);
                        }
                        if (ShouldCancel)
                        {
                            return;
                        }
                        if (x.Command.ToUpper() == "WAIT")
                        {
                            WaitUntil = DateTime.Now.AddSeconds(int.Parse(x.Text));
                        }


                        while (true)
                        {
                            if (ShouldCancel)
                            {
                                return;
                            }

                            switch (x.Command.ToUpper())
                            {
                                case "WAIT":
                                    {
                                        if (DateTime.Now > WaitUntil)
                                        {
                                            goto OutOfLoop;
                                        }
                                        break;
                                    }
                                case "WAITFOR":
                                    {
                                        if (x.Text.Trim().StartsWith("}") && x.Text.Trim().EndsWith("}"))
                                        {
                                            if (Regex.IsMatch(buffer, x.Text.Trim(), RegexOptions.Singleline))
                                            {
                                                buffer = "";
                                                goto OutOfLoop;
                                            }
                                        }
                                        else
                                        {
                                            if (buffer.ToUpper().Contains(x.Text.Trim().ToUpper()))
                                            {
                                                buffer = "";
                                                goto OutOfLoop;
                                            }
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        goto OutOfLoop;
                                    }

                            }


                            Thread.Sleep(1000);
                        };

                    OutOfLoop: ;

                        if (ShouldCancel)
                        {
                            return;
                        }

                    };
                    
                },TaskCreationOptions.LongRunning);
            ShouldCancel = false;
        }

        #region "IPhoneManagerStuff"
        public PhoneStatus AttemptDial(string PhoneNumber)
        {
            return PhoneStatus.Busy;
        }

        public void CallHungUp(string PhoneNumber, string Reason)
        {
            ShouldCancel = true;
        }

        public void SayReceived(string PhoneNumber, string Text)
        {
            CallLogger.LogTwilioSay(Text);
                buffer +=" "+ Text;
        }

        public void SMSReceived(string FromPhoneNumber, string ToPhoneNumber, string Text)
        {
            throw new NotImplementedException();
        }

        public IPhoneInteractionLogger CallLogger
        {
            get
           ;
            set
           ;
        }
        #endregion
        public void InitalizeWithScript(string ScriptText)
        {
            var a = ScriptText.Split(new string[]{Environment.NewLine},StringSplitOptions.RemoveEmptyEntries);
            ParsedScripts = Regex.Matches(ScriptText, "(?<Cmd>.*?):(?<Text>.*?$)", (RegexOptions.Multiline)).Cast<Match>().Select(x =>
                new ParsedTelScript()
                {
                    Command = x.Groups["Cmd"].Value,
                    Text = x.Groups["Text"].Value
                }).ToList();  

        }


       
    }

    public class ParsedTelScript
    {
        public string Command { get; set; }
        public string Text { get; set; }
    }
}
