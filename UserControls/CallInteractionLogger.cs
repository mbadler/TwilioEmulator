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

       

        private void LogInteractionToLog(InteractionWho fromwho, InteractionWhat what, string interaction)
        {
            


                richTextBox1.SelectionColor = fromwho == InteractionWho.Phone ? Color.Red : Color.Blue;
                if (what == InteractionWhat.Dial || what == InteractionWhat.Hangup || what == InteractionWhat.Pickup)
                {
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                }
                else
                {
                    if (what == InteractionWhat.SMS)
                    {
                        richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                        
                        richTextBox1.SelectionColor = Color.Black;
                    }
                    else
                    {
                        richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
                    }
                }

                richTextBox1.AppendText("\n" + DateTime.Now.ToString() + " " + fromwho.ToString() + " " + what.ToString() + " :: " + interaction);
            
         
        }

        private void tmrBuffer_Tick(object sender, EventArgs e)
        {
            LogInteractionToLog(InteractionWho.server, InteractionWhat.Say, WordBuffer);
            tmrBuffer.Enabled = false;
            WordBuffer = "";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; //Set the current caret position at the end
            richTextBox1.ScrollToCaret(); //Now scroll it automatically
        }

        void IPhoneInteractionLogger.LogInteraction(InteractionWho fromwho, InteractionWhat what, Color BackgroundColor, string interaction)
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
                LogInteractionToLog(fromwho, what, interaction);
            }));
        }

        void IPhoneInteractionLogger.LogTwilioSay(string interaction)
        {
            this.BeginInvoke((Action)(() =>
            {
            LogInteractionToLog(InteractionWho.server, InteractionWhat.Say, interaction);
           } ));
        }

        void IPhoneInteractionLogger.LogDigitPressed(string digits)
        {
            this.BeginInvoke((Action)(() =>
            {
            LogInteractionToLog(InteractionWho.Phone, InteractionWhat.Dial, digits);
            }));
        }
    }
}
