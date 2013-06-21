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

        public PhoneStatus AttemptDial(string PhoneNumber)
        {
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

            SystemController.Instance.Logger.Log2Nodes("[] --> }   Dial Phone", "Incoming Number", PhoneNumber, "PhoneStatus", retStatus.ToString(), false);
            return retStatus;
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
                _phoneStatus = value;
            }
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
                case "Answer-Machine":
                    {
                        PhoneStatus = PhoneStatus.ReadyHuman;
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
        }
        bool ringFlipFlop = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
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
            

        }

        private void PickupPhone()
        {
            btnStatus.Text = "Talking (Hang Up)";
            btnStatus.BackColor = Color.PowderBlue;
            PhoneStatus = PhoneStatus.Talking;
            SystemController.Office.PhonePickedUp(currentPhoneNum);
            
        }
    }
}
