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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.trvCallView = new System.Windows.Forms.TreeView();
            this.ilstCallView = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.trvLog = new System.Windows.Forms.TreeView();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.trvHttp = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.ddAnswerMode = new System.Windows.Forms.ToolStripComboBox();
            this.tbtnMakePhoneNoises = new System.Windows.Forms.ToolStripButton();
            this.tbtnAutoPickup = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.lblServerHeader = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.callInteractionLogger1 = new TwilioEmulator.UserControls.CallInteractionLogger();
            this.label1 = new System.Windows.Forms.Label();
            this.touchPadDialer1 = new TwilioEmulator.TouchPadDialer();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(886, 650);
            this.splitContainer1.SplitterDistance = 317;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 40);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(886, 277);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(878, 250);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Call API Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.trvCallView);
            this.splitContainer3.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.trvLog);
            this.splitContainer3.Panel2.Controls.Add(this.label5);
            this.splitContainer3.Size = new System.Drawing.Size(872, 244);
            this.splitContainer3.SplitterDistance = 290;
            this.splitContainer3.TabIndex = 2;
            // 
            // trvCallView
            // 
            this.trvCallView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvCallView.ImageIndex = 0;
            this.trvCallView.ImageList = this.ilstCallView;
            this.trvCallView.Location = new System.Drawing.Point(0, 14);
            this.trvCallView.Name = "trvCallView";
            this.trvCallView.SelectedImageIndex = 0;
            this.trvCallView.Size = new System.Drawing.Size(290, 230);
            this.trvCallView.TabIndex = 1;
            // 
            // ilstCallView
            // 
            this.ilstCallView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilstCallView.ImageStream")));
            this.ilstCallView.TransparentColor = System.Drawing.Color.Transparent;
            this.ilstCallView.Images.SetKeyName(0, "OutBoundQueued.png");
            this.ilstCallView.Images.SetKeyName(1, "OutBoundRinging.png");
            this.ilstCallView.Images.SetKeyName(2, "OutBoundInProg.png");
            this.ilstCallView.Images.SetKeyName(3, "OutboundCompletedOk.png");
            this.ilstCallView.Images.SetKeyName(4, "OutboundCompletedError.png");
            this.ilstCallView.Images.SetKeyName(5, "InBoundQueued.png");
            this.ilstCallView.Images.SetKeyName(6, "InboudRinging.png");
            this.ilstCallView.Images.SetKeyName(7, "InboudInProg.png");
            this.ilstCallView.Images.SetKeyName(8, "InboudCompletedOk.png");
            this.ilstCallView.Images.SetKeyName(9, "InboudCompletedError.png");
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(290, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "Call List";
            // 
            // trvLog
            // 
            this.trvLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvLog.Location = new System.Drawing.Point(0, 14);
            this.trvLog.Name = "trvLog";
            this.trvLog.Size = new System.Drawing.Size(578, 230);
            this.trvLog.TabIndex = 1;
            this.trvLog.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvLog_NodeMouseDoubleClick);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(578, 14);
            this.label5.TabIndex = 2;
            this.label5.Text = "API Log (double click node for more details)";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.trvHttp);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(878, 251);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "HTTP Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // trvHttp
            // 
            this.trvHttp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvHttp.Location = new System.Drawing.Point(3, 3);
            this.trvHttp.Name = "trvHttp";
            this.trvHttp.Size = new System.Drawing.Size(872, 245);
            this.trvHttp.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.lblServerHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(886, 40);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.ddAnswerMode,
            this.tbtnMakePhoneNoises,
            this.tbtnAutoPickup,
            this.toolStripLabel3,
            this.toolStripLabel4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 14);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(886, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
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
            // tbtnMakePhoneNoises
            // 
            this.tbtnMakePhoneNoises.Checked = true;
            this.tbtnMakePhoneNoises.CheckOnClick = true;
            this.tbtnMakePhoneNoises.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbtnMakePhoneNoises.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbtnMakePhoneNoises.Enabled = false;
            this.tbtnMakePhoneNoises.Image = ((System.Drawing.Image)(resources.GetObject("tbtnMakePhoneNoises.Image")));
            this.tbtnMakePhoneNoises.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnMakePhoneNoises.Name = "tbtnMakePhoneNoises";
            this.tbtnMakePhoneNoises.Size = new System.Drawing.Size(108, 22);
            this.tbtnMakePhoneNoises.Text = "Play Phone Noises";
            // 
            // tbtnAutoPickup
            // 
            this.tbtnAutoPickup.Checked = true;
            this.tbtnAutoPickup.CheckOnClick = true;
            this.tbtnAutoPickup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbtnAutoPickup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbtnAutoPickup.Image = ((System.Drawing.Image)(resources.GetObject("tbtnAutoPickup.Image")));
            this.tbtnAutoPickup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnAutoPickup.Name = "tbtnAutoPickup";
            this.tbtnAutoPickup.Size = new System.Drawing.Size(79, 22);
            this.tbtnAutoPickup.Text = "Auto Pick up";
            this.tbtnAutoPickup.Click += new System.EventHandler(this.tbtnAutoPickup_Click);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(47, 22);
            this.toolStripLabel3.Text = "Dial To:";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(78, 22);
            this.toolStripLabel4.Text = "Not Specified";
            // 
            // lblServerHeader
            // 
            this.lblServerHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblServerHeader.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerHeader.Location = new System.Drawing.Point(0, 0);
            this.lblServerHeader.Name = "lblServerHeader";
            this.lblServerHeader.Size = new System.Drawing.Size(886, 14);
            this.lblServerHeader.TabIndex = 0;
            this.lblServerHeader.Text = "Emulator Server - Not active";
            this.lblServerHeader.Click += new System.EventHandler(this.lblServerHeader_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 14);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.callInteractionLogger1);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.touchPadDialer1);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Size = new System.Drawing.Size(886, 315);
            this.splitContainer2.SplitterDistance = 442;
            this.splitContainer2.TabIndex = 2;
            // 
            // callInteractionLogger1
            // 
            this.callInteractionLogger1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.callInteractionLogger1.Location = new System.Drawing.Point(0, 13);
            this.callInteractionLogger1.Name = "callInteractionLogger1";
            this.callInteractionLogger1.Size = new System.Drawing.Size(442, 302);
            this.callInteractionLogger1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(442, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Call Log:";
            // 
            // touchPadDialer1
            // 
            this.touchPadDialer1.CallLogger = null;
            this.touchPadDialer1.DefaultPhoneStatus = TwilioEmulator.Phones.PhoneStatus.ReadyHuman;
            this.touchPadDialer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.touchPadDialer1.Location = new System.Drawing.Point(0, 13);
            this.touchPadDialer1.Margin = new System.Windows.Forms.Padding(0);
            this.touchPadDialer1.Name = "touchPadDialer1";
            this.touchPadDialer1.PhoneNumber = null;
            this.touchPadDialer1.PhoneStatus = TwilioEmulator.Phones.PhoneStatus.ReadyHuman;
            this.touchPadDialer1.Size = new System.Drawing.Size(440, 302);
            this.touchPadDialer1.TabIndex = 2;
            this.touchPadDialer1.Load += new System.EventHandler(this.touchPadDialer1_Load);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(440, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Phone:";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(886, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Phone";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(886, 650);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Twilio Emulator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
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
        private Label label1;
        private Label label3;
        private ImageList ilstCallView;
        private TouchPadDialer touchPadDialer1;
        private ToolStripButton tbtnMakePhoneNoises;
        private ToolStripButton tbtnAutoPickup;
        private UserControls.CallInteractionLogger callInteractionLogger1;
        private ToolStripLabel toolStripLabel3;
        private ToolStripLabel toolStripLabel4;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TreeView trvHttp;
        private SplitContainer splitContainer3;
        private Label label4;
        private Label label5;
        private TreeView trvCallView;
    }
}