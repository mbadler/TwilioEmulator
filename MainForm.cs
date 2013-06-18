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
    public partial class MainForm : Form,ILogger,IPhoneManager,IPhone
    {
        public MainForm()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            SystemController.Instance.theForm = this;
            SystemController.Instance.PhoneManager = this;

            this.lblServerHeader.Text = "Emulator Server - Port: " + SystemController.Instance.ActivePort.ToString();

        }
        
        



        #region ILogger Implemtents
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

        #endregion

        #region IPhoneManager Implements

        

        public PhoneStatus AttemptDial(string PhoneNumber)
        {
            switch (PhoneStatus)
            {
                case PhoneStatus.ReadyHuman:
                case PhoneStatus.ReadyMachine:
                    {
                        PhoneStatus = PhoneStatus.Ringing;
                        break;
                    }
                case PhoneStatus.Talking:
                    {
                        return PhoneStatus.Busy;
                         
                    }
                    
                case PhoneStatus.Busy:
                    break;
                case PhoneStatus.Ringing:
                    {
                        return PhoneStatus.Busy;
                         
                    }
                case PhoneStatus.NotInService:
                    break;
                case PhoneStatus.NoAnswer:
                    break;
                default:
                    break;
            }

            return PhoneStatus;
        }

        public PhoneStatus PhoneStatus
        {
            get;
            set;
        }

        #endregion

        private void ddAnswerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PhoneStatus == PhoneStatus.Talking || PhoneStatus == PhoneStatus.Ringing)
            {
                MessageBox.Show("There currenly is a phone call in progress - please end it before changeing statuses");
                return;
            }

            switch (ddAnswerMode.SelectedItem.ToString())
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

        private void button1_Click(object sender, EventArgs e)
        {
            var v = new NameValueCollection();
            v.Add("asdfasd", "sdafsdf");
            v.Add("dsfsdf", "asdfsd");
            SystemController.Instance.Logger.LogSimpleObject(v, "ff");
        }

       
    }
}
