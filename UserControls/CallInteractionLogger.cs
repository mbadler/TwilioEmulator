using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwilioEmulator.Code;

namespace TwilioEmulator.UserControls
{
    public partial class CallInteractionLogger : UserControl, IPhoneInteractionLogger
    {
        public CallInteractionLogger()
        {
            InitializeComponent();
        }

        string WordBuffer = "";

        public void LogInteraction(InteractionWho fromwho, InteractionWhat what, Color BackgroundColor, string interaction)
        {
            this.BeginInvoke((Action)(() =>
            {
            tmrBuffer.Enabled = true;
            if (fromwho == InteractionWho.server && what == InteractionWhat.Say)
            {
                
                WordBuffer = WordBuffer + " " + interaction;
                if (WordBuffer.Length < 100)
                {
                    tmrBuffer.Enabled = true;
                    return;
                }
            }
            LogInteractionToLog(fromwho, what, WordBuffer);
            WordBuffer = "";}));
        }

        private void LogInteractionToLog(InteractionWho fromwho, InteractionWhat what, string interaction)
        {
            


                richTextBox1.SelectionColor = fromwho == InteractionWho.Phone ? Color.Red : Color.Blue;
                if (what == InteractionWhat.Dial || what == InteractionWhat.Hangup || what == InteractionWhat.Pickup)
                {
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                }
                else
                {
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
                }

                richTextBox1.AppendText("\n" + DateTime.Now.ToString() + " " + fromwho.ToString() + " " + what.ToString() + " " + interaction);
            
         
        }

        private void tmrBuffer_Tick(object sender, EventArgs e)
        {
            LogInteractionToLog(InteractionWho.server, InteractionWhat.Say, WordBuffer);
            tmrBuffer.Enabled = false;
            WordBuffer = "";
        }
    }
}
