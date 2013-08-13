using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwilioEmulator.Code;
using TwilioEmulator.Phones;
using System.Collections.Specialized;
using TwilioEmulator.Offices;

namespace TwilioEmulator
{
    public partial class MainForm : Form,ILogger
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private Office mOffice = null; 

        private void Form1_Load(object sender, EventArgs e)
        {
            SystemController.Instance.theForm = this;
            SystemController.Instance.PhoneManager = touchPadDialer1;
            SystemController.Instance.OnHttpLog += new EventHandler<Code.MessageHandler.HttpLoggingEventArgs>(Instance_OnHttpLog);

            this.lblServerHeader.Text = "Emulator Server - Port: " + SystemController.Instance.ActivePort.ToString();

            touchPadDialer1.phonelog = callInteractionLogger1;
        }

        

        private void ddAnswerMode_Click(object sender, EventArgs e)
        {
            
        }

        private void ddAnswerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            touchPadDialer1.ChangePhoneStatus(ddAnswerMode.SelectedItem.ToString());
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private static bool _AutoPickup = true;

        public static bool AutoPickup
        {
            get { return _AutoPickup; }
            set { _AutoPickup = value; }
        }
        
        

        private void tbtnAutoPickup_Click(object sender, EventArgs e)
        {
            AutoPickup = tbtnAutoPickup.Checked;
        }



        public void LogObj(LogObj logObject)
        {
            this.BeginInvoke((Action)(() =>
            {
                TreeNode tn = new TreeNode();
                string nodeTxt = DateTime.Now.ToString() + " " + logObject.LogSymbol.ToDisplayString() + " " + logObject.Caption;
                if (logObject.CallInstance != null)
                {
                   nodeTxt = nodeTxt + " (" + logObject.CallInstance.CallOptions.From + "->" + logObject.CallInstance.CallOptions.To + ")";
                   tn.BackColor = logObject.CallInstance.CallColor;
                }

                tn.Text = nodeTxt;

                trvLog.Nodes.Add(tn);
                logObject.logObjs.ToList().ForEach(x =>
                 {
                     tn.Nodes.Add(new TreeNode(x.Key + " : " + this.GetObjectText(x.Value)));
                 }
                 );

                tn.EnsureVisible();
            }));
        }

        private void lblServerHeader_Click(object sender, EventArgs e)
        {

        }

        


        #region Header Managment

internal void ControlerCreated()
        {
            mOffice = SystemController.Instance.Office;
            mOffice.IncomingPhoneNumberChanged += new EventHandler<StringEventArgs>(Office_IncomingPhoneNumberChanged);
            DisplayIncomingPhonenumber();
            
        }

       

        protected void DisplayIncomingPhonenumber()
        {
            toolStripLabel4.Text = mOffice.DefaultNumber.PhoneNumber;
        }

        #endregion

        #region External Handler Implementations
        void Instance_OnHttpLog(object sender, Code.MessageHandler.HttpLoggingEventArgs e)
        {
             this.BeginInvoke((Action)(() =>
            {
            if (e.Direction == "In")
            {
                trvHttp.Nodes.Add("-> " + DateTime.Now.ToString() + " " + e.Method + " " + e.Uri).Name = e.CorrelationID;
                return;
            }

            if (e.Direction == "Out")
            {
                var nd = trvHttp.Nodes.Find(e.CorrelationID,false);
                var ind = nd.First();
                if (e.Result == "OK")
                {
                    ind.Text = ind.Text.Replace("->", "<>") + " - Ok";
                }
                else
                {
                    ind.Text = ind.Text.Replace("->", "XX") + " - " + e.Result;
                }
            }
            }));

        }

        void Office_IncomingPhoneNumberChanged(object sender, StringEventArgs e)
        {
            DisplayIncomingPhonenumber();
        }
        #endregion



    }


}
