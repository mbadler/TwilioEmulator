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

namespace TwilioEmulator
{
    public partial class MainForm : Form,ILogger
    {
        public MainForm()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            SystemController.Instance.theForm = this;
            SystemController.Instance.PhoneManager = touchPadDialer1;

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
    }
}
