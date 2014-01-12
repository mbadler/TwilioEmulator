using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwilioEmulator.Phones;
using TwilioEmulator.Code;
using TwilioEmulator.Properties;
using TwilioEmulator.Code.ScriptEngines;
using System.IO;

namespace TwilioEmulator
{
    public partial class TouchPadDialer : UserControl, IPhoneManager, IPhone
    {
        private ScriptRunner scrun = null;

        private string outgoingPhoneNumber = "";
        public TouchPadDialer()
        {
            InitializeComponent();
            SystemController.Instance.Office.IncomingPhoneNumberChanged += new EventHandler<StringEventArgs>(Office_IncomingPhoneNumberChanged);
            SystemController.Instance.Office.OutgoingPhoneNumberChanged += new EventHandler<StringEventArgs>(Office_OutgoingPhoneNumberChanged);
        
        }

        void Office_IncomingPhoneNumberChanged(object sender, StringEventArgs e)
        {
            outgoingPhoneNumber = e.value;
            ResetPhone();
             
        }

        void Office_OutgoingPhoneNumberChanged(object sender, StringEventArgs e)
        {
            PhoneNumber = e.value;
        }


        #region IPhoneManager Implements



        public IPhoneInteractionLogger CallLogger
        {
            get
            ;
            set
            ;
        }
        

        public PhoneStatus AttemptDial(string PhoneNumber)
        {
            CallLogger.LogInteraction(InteractionWho.server, InteractionWhat.Dial, SystemColors.Desktop, PhoneNumber);
            PhoneStatus retStatus = PhoneStatus.Ringing;

            currentPhoneNum = PhoneNumber;

            switch (PhoneStatus)
            {   case PhoneStatus.Ringing:
                case PhoneStatus.Busy:
                case PhoneStatus.Talking:
                    {
                        retStatus = PhoneStatus.Busy;
                        break;

                    }





                case PhoneStatus.NotInService:
                    {
                        retStatus = PhoneStatus.NotInService;
                        break;
                    }
                case PhoneStatus.NoAnswer:
                    {
                        retStatus = PhoneStatus.NoAnswer;
                    }
                    break;
                default:
                    {
                        PhoneStatus = PhoneStatus.Ringing;
                    }
                    break;
            }


            CallLogger.LogInteraction(InteractionWho.Phone, InteractionWhat.Pickup, SystemColors.Desktop, retStatus.ToString());
             return retStatus;
        }

         public void CallHungUp(string PhoneNumber, string Reason)
        {
            this.BeginInvoke((Action)(() =>
          {
              PhoneStatus = DefaultPhoneStatus;
          }));
        }
         void IPhoneManager.SayReceived(string PhoneNumber, string Text)
         {
             CallLogger.LogInteraction(InteractionWho.server, InteractionWhat.Say, SystemColors.Desktop, Text);
         }

         void IPhoneManager.SMSReceived(string FromPhoneNumber, string ToPhoneNumber, string Text)
         {
             CallLogger.LogInteraction(InteractionWho.server, InteractionWhat.SMS, Color.Green, "SMS - From:" + FromPhoneNumber + "  " + Text);
         }
         
        #endregion 

  string currentPhoneNum = "";
        private PhoneStatus _defaultPhoneStatus = PhoneStatus.ReadyHuman;

         public PhoneStatus DefaultPhoneStatus
         {
             get { return _defaultPhoneStatus; }
             set { _defaultPhoneStatus = value; }
         }
        PhoneStatus _phoneStatus = PhoneStatus.ReadyHuman; 
        public PhoneStatus PhoneStatus
        {
            get
            {
                return _phoneStatus;
            }
            set
            {
                if (value == _phoneStatus)
                {
                    return;
                }
                timer1.Enabled = false;
                if (value == PhoneStatus.Ringing)
                {
                    StartPhoneRinging();
                }
                if (value == DefaultPhoneStatus)
                {
                    // we are assuming that the phone was hung up 
                    ResetPhone();
                }
                
                _phoneStatus = value;
            }
        }

        private void ResetPhone()
        {
            btnStatus.BackColor = Color.Transparent;
            btnStatus.Text = "Dial Number ("+outgoingPhoneNumber+")";
            PhoneNumber = outgoingPhoneNumber;
        }

        

        public void StartPhoneRinging()
        {
            this.BeginInvoke((Action)(() =>
            {
                timer1.Enabled = true;
                btnStatus.Text = "Ringing (Pick up)";
                //Console.WriteLine("-- " + DateTime.Now.ToString() + " " + name);
               
            }));
            
           
        }

        public void ChangePhoneStatus(string PhoneStatusText)
        {
            if (PhoneStatus == PhoneStatus.Talking || PhoneStatus == PhoneStatus.Ringing)
            {
                MessageBox.Show("There currently is a phone call in progress - please end it before changing statuses");
                return;
            }
            
            switch (PhoneStatusText)
            {
                case "Answer-Human":
                    {
                        PhoneStatus = PhoneStatus.ReadyHuman;
                        break;
                    }
                case "Answer-Machine":
                    {
                        PhoneStatus = PhoneStatus.ReadyMachine;
                        break;
                    }
                case "No Answer":
                    {
                        PhoneStatus = PhoneStatus.NoAnswer;
                        break;
                    }
                case "Busy":
                    {
                        PhoneStatus = PhoneStatus.Busy;
                        break;
                    }
                case "Not In Service":
                    {
                        PhoneStatus = PhoneStatus.NotInService;
                        break;
                    }
            }
            DefaultPhoneStatus = PhoneStatus;
        }
        bool ringFlipFlop = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MainForm.AutoPickup)
            {
              
                    PickupPhone();
                    return;
              
            }
            ringFlipFlop = !ringFlipFlop;
            btnStatus.BackColor = (ringFlipFlop) ? SystemColors.Control : Color.Red;
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            switch (PhoneStatus)
            {
                case PhoneStatus.Ringing:
                    {
                        PickupPhone();
                        break;
                    }


                case PhoneStatus.Talking:
                    {
                        HangupPhone();
                        break;

                    }
                case PhoneStatus.ReadyHuman:
                    {
                        btnStatus.Text = "Talking (Hang Up)";
                        btnStatus.BackColor = Color.PowderBlue;
                        PhoneStatus = PhoneStatus.Talking;
                        CallLogger.LogInteraction(InteractionWho.Phone, InteractionWhat.Pickup, SystemColors.Desktop, "");
                        currentPhoneNum = outgoingPhoneNumber;
                        SystemController.Instance.Office.PhoneDialingIn( Settings.Default.DefaultFromNumber,outgoingPhoneNumber);
                        break;
                    }
                
            }
        }

        private void HangupPhone()
        {
            PhoneStatus = DefaultPhoneStatus;
            SystemController.Instance.Office.PhoneHungUp(outgoingPhoneNumber);
            CallLogger.LogInteraction(InteractionWho.Phone, InteractionWhat.Hangup, SystemColors.Desktop, "");

        }

        private void PickupPhone()
        {
            btnStatus.Text = "Talking (Hang Up)";
            btnStatus.BackColor = Color.PowderBlue;
            PhoneStatus = PhoneStatus.Talking;
            CallLogger.LogInteraction(InteractionWho.Phone, InteractionWhat.Pickup, SystemColors.Desktop, "");
            SystemController.Instance.Office.PhonePickedUp(currentPhoneNum,DefaultPhoneStatus == PhoneStatus.ReadyMachine?true:false);
            
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            var tpress = b.Text.First();
            if (lblBuffer.Text == "")
            {
                lblBuffer.Text = "Buffer: ";
                lblBuffer.Visible = true;
            }

            lblBuffer.Text = lblBuffer.Text + tpress;
            tmrDial.Enabled = false;
            tmrDial.Enabled = true;
            if (tpress == '#' || tpress == '*')
            {
                SubmitBuffer();
            }
           
        }

        private void SubmitBuffer()
        {
            CallLogger.LogInteraction(InteractionWho.Phone, InteractionWhat.Say, SystemColors.Desktop, lblBuffer.Text.Replace("Buffer: ", ""));
            tmrDial.Enabled = false;

            SystemController.Instance.Office.PhoneSendingDigits(PhoneNumber, lblBuffer.Text.Replace("Buffer: ", ""));
         lblBuffer.Text = "";
            lblBuffer.Visible = false;
        }

        private void tmrDial_Tick(object sender, EventArgs e)
        {
            SubmitBuffer();
        }



        public string PhoneNumber { get; set; }

        private void tbTouchPad_TabIndexChanged(object sender, EventArgs e)
        {
            if (tbTouchPad.TabIndex == 0)
            {
                SystemController.Instance.PhoneManager = this;
            }
            else
            {
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Dial and start")
            {
                scrun = new ScriptRunner();
                scrun.CallLogger = CallLogger;

                SystemController.Instance.PhoneManager = scrun;
                scrun.PhoneNumber = outgoingPhoneNumber;
                scrun.InitalizeWithScript(txtScript.Text);
                SystemController.Instance.Office.PhoneDialingIn(Settings.Default.DefaultFromNumber, outgoingPhoneNumber);
                scrun.Run();
                button1.Text = "Hang Up";
            }
            else 
            {
                scrun.ShouldCancel = true;
                
                    HangupPhone();
               
                button1.Text = "Dial and start";
            }
        }

        private void TouchPadDialer_Load(object sender, EventArgs e)
        {
            // try to load the script into the text box 
            if (File.Exists("Script.Txt"))
            {
                txtScript.Text = File.ReadAllText("Script.Txt");
            }
        }

        private void txtScript_TextChanged(object sender, EventArgs e)
        {
            if (chkAutoSaveScript.Checked)
            {
                File.WriteAllText("Script.Txt", txtScript.Text);
            }
            
        }




        
    }
}
