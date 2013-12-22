using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwilioEmulator.Code;
using TwilioEmulator.Code.Extensions;
using TwilioEmulator.Phones;
using Twilio;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using TwilioEmulator.Code.Models;
using System.Text.RegularExpressions;
using TwilioEmulator.Properties;

namespace TwilioEmulator.Offices
{
    public class Office:OfficeBase
    {

      

        public List<CallInstance> CallList = new List<CallInstance>();


        public IncomingPhoneNumber DefaultNumber = new IncomingPhoneNumber();

        #region API Requests

        public CallInstance NewCallRequest(CallOptions co)
        {
            return CreateACall(co,false);
        }

        private CallInstance CreateACall(CallOptions co,bool IsInbound)
        {

            // preassign some info to the call if it has not already been specified
            if ( co.StatusCallback == null)
            {
                co.StatusCallback = Settings.Default.StatusCallBackUrl;
            }
            
            CallInstance c = new CallInstance()
            {

                
                CallForCreate = new Call()
                {
                    Sid = CreateSid(),
                    Status = TwilioCallStatuses.QUEUED,
                    Direction = IsInbound?"inbound":"outbound-api",
                    DateCreated = DateTime.Now.ToUniversalTime(),
                    DateUpdated = DateTime.Now.ToUniversalTime(),
                    
                    To = co.To,
                    From = co.From,
                    
                    

                },
                CallDirection = IsInbound?CallDirection.In: CallDirection.Out,
                CallOptions = co,

                

            };

            CallList.Add(c);
            return c;
        }

private static string CreateSid()
{
return Guid.NewGuid().ToString().Replace("-", "");
}

        private void HangupCallRequest(CallInstance d, string stat)
        {
            HangUpThePhoneConnectionFromTheserver(d, stat);
        }
        #endregion

        public void DefualtNumberUpdated()
        {
            if (IncomingPhoneNumberChanged != null)
            {
                IncomingPhoneNumberChanged(this, new StringEventArgs(DefaultNumber.PhoneNumber));
            }
        }


        protected override void Process()
        {
            // run thru each call and see what needs to be done with them
       
            CallList.Where(x=>
                x.CallStatus==CallStatus.ReadyForProcessing||
                x.CallStatus==CallStatus.Queued||
                x.CallStatus==CallStatus.WaitingRinging
                ).AsParallel().ForAll(x=>
                {
                    ProcessCallInstance(x);
                });
            
        }

        void ProcessCallInstance(CallInstance ci)
        {
            if (ci.CallStatus == CallStatus.Queued && ci.CallDirection == CallDirection.Out)
            {
               // start the phone call
                ProcessCallPhone(ci);
            }

            if (ci.CallStatus == CallStatus.ReadyForProcessing || 
                (ci.CallStatus == CallStatus.Queued && ci.CallDirection == CallDirection.In))
            {
                // we are processing twiml
                ci.CallForSet.StartTime = DateTime.Now.ToUniversalTime();
                ci.CallStatus = CallStatus.ReadyForProcessing;
                ci.CallForSet.Status = TwilioCallStatuses.INPROGRESS;
                ci.FlowEngine.StartCallFlow();
            }

            
            
        }

 #region Phone Interations       

        private void ProcessCallPhone(CallInstance ci)
        {
            // when we go to multi phones we will look up by phone number etc... - but for now 
            // we will look at the first phone
            // if there is no first phone then fire the need phone eventhandler
            
            Boolean Callok = false;

            
            var c = ci.CallForGet;
            c.StartTime =  DateTime.Now.ToUniversalTime();
            var v = SystemController.Instance.PhoneManager.AttemptDial(ci.CallForGet.To);
            switch (v)
            {

                case PhoneStatus.Busy:
                    {
                        ci.CallForSet.Status = TwilioCallStatuses.BUSY;

                        break;
                    }
                case PhoneStatus.Ringing:
                    {
                        Callok = true;
                        ci.CallForSet.Status = TwilioCallStatuses.RINGING;
                        break;
                    }
                case PhoneStatus.NotInService:
                    {
                        ci.CallForSet.Status = TwilioCallStatuses.FAILED;
                        break;
                    }
                case PhoneStatus.NoAnswer:
                    {
                        ci.CallForSet.Status = TwilioCallStatuses.NOANSWER;
                        break;
                    }
                default:
                    break;
            }

           

            // if it is not a good call send back a error call status and mark the call done
            // else mark it as waiting for the phone
            if (!Callok)
            {
                MarkCallEnded(ci,"Phone in status "+c.Status.ToString(),false);
               
            }
            else
            {
                ci.CallStatus = CallStatus.WaitingRinging;
                ci.CallForGet.Status = TwilioCallStatuses.RINGING;
            }

             
            SystemController.Instance.Logger.LogObj(new LogObj()
            {
                Caption = "Dial to Phone",
                LogSymbol = LogSymbol.TwilioToPhone,
                CallInstance = ci

            }.AddNode("Incoming Number", ci.CallOptions.To).AddNode("Phone Status", v.ToString()));
        }

        public void PhoneHungUp(string phonenumber)
        {
            var a = GetCallInstanceFromPhoneNumber(phonenumber);
            var v = new LogObj()
            {
                Caption = "X Phone hung up call",
                LogSymbol = Code.LogSymbol.PhoneToTwilio,
                CallInstance = a
            };


            SystemController.Instance.Logger.LogObj(v);
            MarkCallEnded(a,"Hung up",false);
            
            
        }


        public void MarkCallEnded(CallInstance ci,string reason,bool FromTheServer)
        {
            // run this in its own thread to ensure it runs

            Task.Factory.StartNew(() =>
                {
                    var a = ci.CallForSet;
                    if (a.Status == TwilioCallStatuses.INPROGRESS || a.Status== TwilioCallStatuses.QUEUED)
                    {
                        a.EndTime = DateTime.Now.ToUniversalTime();
                        a.Duration = Convert.ToInt32((a.EndTime.Value - a.DateCreated).TotalSeconds);
                        a.Status = TwilioCallStatuses.COMPLETED;
                        // set the price on the call
                        var nummins = Convert.ToInt32(Math.Floor( (a.EndTime.Value - ci.CallForGet.StartTime).Value.TotalMinutes)) +1;
                        var price = 2.0; //default to outbound
                        if (a.Direction == "inbound")
                        {
                            price = Regex.IsMatch(a.To,@"1(\s?|-?)800(\s?|-?)\d{3}(\s?|-?)\d{4}")?4:1;
                        }
                        a.Price =  (decimal)price * (decimal)nummins;
                    }

                    ci.AbortCallFlow();
               
                    
                    ci.CallStatus = CallStatus.Ended;



                    if (FromTheServer)
                    {
                        HangUpThePhoneConnectionFromTheserver(ci, reason);
                    }
                    SendCallEndStatus(ci, ci.GenerateCallBackValue(), reason);
                   

                });
        }


        public void PhoneSendingDigits(string phonenumber, string digits)
        {
            var ci = GetCallInstanceFromPhoneNumber(phonenumber);
            ci.Digits = ci.Digits + digits;
        }

        public void PhonePickedUp(string phonenumber, bool IsMachine)
        {
            // find the ci with that phone number
            var a = GetCallInstanceFromPhoneNumber(phonenumber);
            

          

            if (a.CallOptions.IfMachine != "")
            {
                if (!IsMachine)
                {
                    a.CallForSet.AnsweredBy = "Human";
                }
                else
                {
                    a.CallForSet.AnsweredBy = "Machine";
                    if (a.CallOptions.IfMachine == "Hangup")
                    {
                        
                        MarkCallEnded(a,"Answered by answering machine",true);
                       
                        
                        return;
                    }

                }
            }


            a.CallStatus = CallStatus.ReadyForProcessing;
            a.CallForSet.Status = TwilioCallStatuses.INPROGRESS;
            //ProcessDoTwimlRequest(a, a.CallOptions.Url, "Get Welcome message Twiml");
            var v = new LogObj()
            {
                Caption = "^ Phone Picked Up " + (IsMachine ? " Machine" : ""),
                LogSymbol = LogSymbol.PhoneToTwilio,
                CallInstance = a
            };
            SystemController.Instance.Logger.LogObj(v);
        }


        public void PhoneDialingIn(string FromPhoneNumber, string toPhoneNumber)
        {
            var v = new CallOptions()
            {
                From = FromPhoneNumber,
                To = toPhoneNumber,
                StatusCallback = DefaultNumber.StatusCallback,
                Url = DefaultNumber.VoiceUrl
            };
            CreateACall(v, true);

        }

        private void HangUpThePhoneConnectionFromTheserver(CallInstance ci,string Reason)
        {
            var v = new LogObj()
            {
                Caption = "X Server hung up call " + Reason,
                LogSymbol = Code.LogSymbol.TwilioToPhone,
                CallInstance = ci
            }.LogIt();

 
            SystemController.Instance.PhoneManager.CallHungUp(ci.CallOptions.To, Reason);

        }

        public void SayToPhone(CallInstance ci, string Word)
        {
            SystemController.Instance.PhoneManager.SayReceived(ci.CallOptions.To, Word);
        }


 #endregion

        protected string SendCallEndStatus(CallInstance ci, NameValueCollection nvc,string Reason)
        {
            ci.CallStatus = CallStatus.CallbackSent;
            var a = "";

            var v = new LogObj()
            {
                LogSymbol = LogSymbol.TwilioToWeb,
                CallInstance = ci,
                Caption = "Call End Status to " + ci.CallOptions.StatusCallback + " " + Reason 
            };


            try
            {
                v.AddNode("Request", nvc);
                 a = ci.BrowserClient.DoRequest(ci.CallOptions.StatusCallback, nvc);
                v.AddNode("Response", a);
                
            }
            catch (Exception ex)
            {
                v.Caption = "<- Exception on Call Start Call Back to " + ci.CallOptions.StatusCallback + ex.Message;
                v.AddException(ex);
            }
            SystemController.Instance.Logger.LogObj(v);
            ci.CallStatus = CallStatus.Ended;
            return a;
        }

       
        
        #region Search Phone List
        public CallInstance GetCallInstanceFromPhoneNumber(string phonenumber)
        {
            var a = CallList.Where(x => (x.CallForGet.To == phonenumber && x.CallStatus != CallStatus.Ended)).First();
            return a;
        }

        public CallInstance GetCallInstanceFromCallSid(string sid)
        {
            sid = sid.ToUpper();
            var a = CallList.Where(x => (x.CallForGet.Sid.ToUpper()==sid)).FirstOrDefault();
            return a;
        }
#endregion
        #region Events
        public event EventHandler<StringEventArgs> IncomingPhoneNumberChanged;
        public event EventHandler<StringEventArgs> OutgoingPhoneNumberChanged;

        #endregion
        #region SMS
        public SMSMessage NewSMSRequest(SMSOptions options)
        {
            var v = new LogObj()
            {
                LogSymbol = LogSymbol.TwilioToPhone,
                Caption = "Sms Message"
            }.AddNode("Sms Message", options.body).LogIt();

            SystemController.Instance.PhoneManager.SMSReceived(options.from, options.to, options.body);

            CallStatusWebClient cswb = new CallStatusWebClient();

            if (options.statusCallback != null)
            {


                cswb.DoRequest(options.statusCallback,
                    new NameValueCollection()
                    {
                        {"SmsStatus","sent"},
                        {"SmsSid",CreateSid()}
                    });
            }
            return new SMSMessage()
            {
                DateCreated = DateTime.Now,
                DateSent = DateTime.Now,
                Direction = "outbound-api",
                From = options.from,
                To = options.to,
                Status = "sent",
                Sid = Guid.NewGuid().ToString().Replace("-", "")
            };
        }
        #endregion


    }

   

}
