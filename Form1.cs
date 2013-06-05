using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwilioEmulator.Code;

namespace TwilioEmulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SystemController.Instance.theForm = this;
            SystemController.Instance.StartUp();
            this.lblServerHeader.Text = "Emulator Server - Port: " + SystemController.Instance.ActivePort.ToString();
        }

        private void lblServerHeader_Click(object sender, EventArgs e)
        {
        }

        internal void LogServerMessage(string Text)
        {
            this.BeginInvoke((Action)(() => this.rtbServer.AppendText("\n" + DateTime.Now.ToString() + " " + Text)));
        }

       
    }
}
