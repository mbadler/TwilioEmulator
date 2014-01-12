using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwilioEmulator.Dialogs
{
    public partial class APIDetail : Form
    {
        public APIDetail()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public static void ShowNode(TreeNode nd)
        {
            var v = new APIDetail();
            if (nd.Nodes.Count > 0)
            {
                v.textBox1.Text = nd.Nodes[0].Text;
            }
            if (nd.Nodes.Count > 01)
            {
                v.textBox2.Text = nd.Nodes[1].Text;
            }
           // v.textBox1.Text = nd.ch
            v.ShowDialog();
        }
    }
}
