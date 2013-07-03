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

namespace TwilioEmulator
{
    public partial class TouchPadDialer : UserControl, IPhoneManager, IPhone
    {
        public TouchPadDialer()
        {
            InitializeComponent();
        }


        #region IPhoneManager Implements

        string currentPhoneNum = "";

        public IPhoneInteractionLogger phonelog = null;
        

        public PhoneStatus AttemptDial(string PhoneNumber)
        {
            phonelog.LogInteraction(InteractionWho.server, InteractionWhat.Dial, SystemColors.Desktop, PhoneNumber);
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


            phonelog.LogInteraction(InteractionWho.Phone, InteractionWhat.Pickup, SystemColors.Desktop, retStatus.ToString());
             return retStatus;
        }

         public void CallHungUp(string PhoneNumber, string Reason)
        {
            this.BeginInvoke((Action)(() =>
          {
              PhoneStatus = DefaultPhoneStatus;
          }));
        }


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
            btnStatus.Text = "Dial Number";
        }

        #endregion

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
                MessageBox.Show("There currenly is a phone call in progress - please end it before changeing statuses");
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
            }
        }

        private void HangupPhone()
        {
            PhoneStatus = DefaultPhoneStatus;
            SystemController.Instance.Office.PhoneHungUp(currentPhoneNum);
            phonelog.LogInteraction(InteractionWho.Phone, InteractionWhat.Hangup, SystemColors.Desktop,"");

        }

        private void PickupPhone()
        {
            btnStatus.Text = "Talking (Hang Up)";
            btnStatus.BackColor = Color.PowderBlue;
            PhoneStatus = PhoneStatus.Talking;
            phonelog.LogInteraction(InteractionWho.Phone, InteractionWhat.Pickup, SystemColors.Desktop, "");
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
            phonelog.LogInteraction(InteractionWho.Phone, InteractionWhat.Say, SystemColors.Desktop, lblBuffer.Text);
            tmrDial.Enabled = false;
            lblBuffer.Text = "";
            lblBuffer.Visible = false;
        }

        private void tmrDial_Tick(object sender, EventArgs e)
        {
            SubmitBuffer();
        }


       
    }
}
