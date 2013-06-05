using System.Windows.Forms;
using System.Drawing;
using System;
namespace TwilioEmulator
{
    partial class Form1
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
            this.splitContainer1 = new SplitContainer();
            this.rtbServer = new RichTextBox();
            this.panel1 = new Panel();
            this.toolStrip1 = new ToolStrip();
            this.lblServerHeader = new Label();
            this.label2 = new Label();
            this.toolStripLabel1 = new ToolStripLabel();
            this.splitContainer1.BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add((Control)this.rtbServer);
            this.splitContainer1.Panel1.Controls.Add((Control)this.panel1);
            this.splitContainer1.Panel2.Controls.Add((Control)this.label2);
            this.splitContainer1.Size = new Size(681, 510);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 0;
            this.rtbServer.BorderStyle = BorderStyle.None;
            this.rtbServer.Dock = DockStyle.Fill;
            this.rtbServer.Location = new Point(0, 40);
            this.rtbServer.Name = "rtbServer";
            this.rtbServer.Size = new Size(681, 287);
            this.rtbServer.TabIndex = 1;
            this.rtbServer.Text = "";
            this.panel1.Controls.Add((Control)this.toolStrip1);
            this.panel1.Controls.Add((Control)this.lblServerHeader);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(681, 40);
            this.panel1.TabIndex = 0;
            this.toolStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.toolStripLabel1
      });
            this.toolStrip1.Location = new Point(0, 14);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(681, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.lblServerHeader.Dock = DockStyle.Top;
            this.lblServerHeader.Font = new Font("Calibri", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblServerHeader.Location = new Point(0, 0);
            this.lblServerHeader.Name = "lblServerHeader";
            this.lblServerHeader.Size = new Size(681, 14);
            this.lblServerHeader.TabIndex = 0;
            this.lblServerHeader.Text = "Emulator Server - Not active";
            this.lblServerHeader.Click += new EventHandler(this.lblServerHeader_Click);
            this.label2.Dock = DockStyle.Top;
            this.label2.Font = new Font("Calibri", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.label2.Location = new Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(681, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Emulator Clients";
            this.toolStripLabel1.IsLink = true;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(246, 22);
            this.toolStripLabel1.Text = "http://localhost:18080/2010-04-01/Accounts/";
         
            this.AutoScaleDimensions = new SizeF(6f, 14f);
          
            this.ClientSize = new Size(681, 510);
            this.Controls.Add((Control)this.splitContainer1);
            this.Font = new Font("Calibri", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.Name = "Form1";
            this.Text = "Twilio Emulator";
            this.Load += new EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

      
        private SplitContainer splitContainer1;
        private Panel panel1;
        private Label lblServerHeader;
        private Label label2;
        private ToolStrip toolStrip1;
        private RichTextBox rtbServer;
        private ToolStripLabel toolStripLabel1;
    }
}