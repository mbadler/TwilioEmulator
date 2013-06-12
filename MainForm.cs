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
    public partial class MainForm : Form,ILogger
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SystemController.Instance.theForm = this;
            
            this.lblServerHeader.Text = "Emulator Server - Port: " + SystemController.Instance.ActivePort.ToString();
        }

        private void lblServerHeader_Click(object sender, EventArgs e)
        {
        }

        //internal void LogServerMessage(string Text)
        //{
        //    this.BeginInvoke((Action)(() => this.rtbServer.AppendText("\n" + DateTime.Now.ToString() + " " + Text)));
        //}

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }




    



      

        void ILogger.LogSimpleObject(object obj, string name)
        {
            this.BeginInvoke((Action)(() =>
            {
                TreeNode tn = new TreeNode(DateTime.Now.ToString() + " " + Name);
                trvLog.Nodes.Add(tn);
                TreeNode tnc = new TreeNode(this.GetObjectText(obj));
                tn.Nodes.Add(tnc);
                //Console.WriteLine("-- " + DateTime.Now.ToString() + " " + name);
            }));
        }

    

        void ILogger.LogLine(string text)
        {this.BeginInvoke((Action)(() =>
            {
                TreeNode tn = new TreeNode(DateTime.Now.ToString() + " " + text);
                trvLog.Nodes.Add(tn);
                //Console.WriteLine("-- " + DateTime.Now.ToString() + " " + name);
            }));
            
        }


        void ILogger.LogDictionaryOfObjects(string name, Dictionary<string, object> LogObj)
        {
            this.BeginInvoke((Action)(() =>
            {
                TreeNode tn = new TreeNode(DateTime.Now.ToString() + " " + name);
                trvLog.Nodes.Add(tn);
                LogObj.ToList().ForEach(x =>
                    {
                        tn.Nodes.Add(new TreeNode(x.Key + " : " + this.GetObjectText(x.Value)));
                    });
            }));
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
    }
}
