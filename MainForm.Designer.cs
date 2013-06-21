using System.Windows.Forms;
using System.Drawing;
using System;
namespace TwilioEmulator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trvLog = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.ddAnswerMode = new System.Windows.Forms.ToolStripComboBox();
            this.lblServerHeader = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.touchPadDialer1 = new TwilioEmulator.TouchPadDialer();
            this.tbtnMakePhoneNoises = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trvLog);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(681, 650);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.TabIndex = 0;
            // 
            // trvLog
            // 
            this.trvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvLog.Location = new System.Drawing.Point(0, 40);
            this.trvLog.Name = "trvLog";
            this.trvLog.Size = new System.Drawing.Size(681, 284);
            this.trvLog.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.lblServerHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(681, 40);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.ddAnswerMode,
            this.tbtnMakePhoneNoises});
            this.toolStrip1.Location = new System.Drawing.Point(0, 14);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(681, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.IsLink = true;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(246, 22);
            this.toolStripLabel1.Text = "http://localhost:18080/2010-04-01/Accounts/";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(103, 22);
            this.toolStripLabel2.Text = "Call Answer Mode";
            // 
            // ddAnswerMode
            // 
            this.ddAnswerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddAnswerMode.DropDownWidth = 150;
            this.ddAnswerMode.Items.AddRange(new object[] {
            "Answer-Human",
            "Answer-Machine",
            "No Answer",
            "Busy",
            "Not In Service"});
            this.ddAnswerMode.Name = "ddAnswerMode";
            this.ddAnswerMode.Size = new System.Drawing.Size(125, 25);
            this.ddAnswerMode.SelectedIndexChanged += new System.EventHandler(this.ddAnswerMode_SelectedIndexChanged);
            this.ddAnswerMode.Click += new System.EventHandler(this.ddAnswerMode_Click);
            // 
            // lblServerHeader
            // 
            this.lblServerHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblServerHeader.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerHeader.Location = new System.Drawing.Point(0, 0);
            this.lblServerHeader.Name = "lblServerHeader";
            this.lblServerHeader.Size = new System.Drawing.Size(681, 14);
            this.lblServerHeader.TabIndex = 0;
            this.lblServerHeader.Text = "Emulator Server - Not active";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 14);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.richTextBox1);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.touchPadDialer1);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Size = new System.Drawing.Size(681, 308);
            this.splitContainer2.SplitterDistance = 328;
            this.splitContainer2.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 13);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(328, 295);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(328, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Call Log:";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(349, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Phone:";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(681, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Phone";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // touchPadDialer1
            // 
            this.touchPadDialer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.touchPadDialer1.Location = new System.Drawing.Point(0, 13);
            this.touchPadDialer1.Margin = new System.Windows.Forms.Padding(0);
            this.touchPadDialer1.Name = "touchPadDialer1";
            this.touchPadDialer1.PhoneStatus = TwilioEmulator.Phones.PhoneStatus.ReadyHuman;
            this.touchPadDialer1.Size = new System.Drawing.Size(349, 295);
            this.touchPadDialer1.TabIndex = 2;
            // 
            // tbtnMakePhoneNoises
            // 
            this.tbtnMakePhoneNoises.Checked = true;
            this.tbtnMakePhoneNoises.CheckOnClick = true;
            this.tbtnMakePhoneNoises.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbtnMakePhoneNoises.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbtnMakePhoneNoises.Image = ((System.Drawing.Image)(resources.GetObject("tbtnMakePhoneNoises.Image")));
            this.tbtnMakePhoneNoises.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnMakePhoneNoises.Name = "tbtnMakePhoneNoises";
            this.tbtnMakePhoneNoises.Size = new System.Drawing.Size(108, 22);
            this.tbtnMakePhoneNoises.Text = "Play Phone Noises";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(681, 650);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Twilio Emulator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

      
        private SplitContainer splitContainer1;
        private Panel panel1;
        private Label lblServerHeader;
        private Label label2;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private TreeView trvLog;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox ddAnswerMode;
        private SplitContainer splitContainer2;
        private RichTextBox richTextBox1;
        private Label label1;
        private Label label3;
        private ImageList imageList1;
        private TouchPadDialer touchPadDialer1;
        private ToolStripButton tbtnMakePhoneNoises;
    }
}