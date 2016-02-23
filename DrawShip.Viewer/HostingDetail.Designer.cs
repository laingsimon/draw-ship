namespace DrawShip.Viewer
{
	partial class HostingDetail
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HostingDetail));
			this.btnClose = new System.Windows.Forms.Button();
			this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
			this.lblHostingDetail = new System.Windows.Forms.LinkLabel();
			this.icoSystemTray = new System.Windows.Forms.NotifyIcon(this.components);
			this.mnuTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.itmShowDetail = new System.Windows.Forms.ToolStripMenuItem();
			this.itmExit = new System.Windows.Forms.ToolStripMenuItem();
			this.lnkGitHub = new System.Windows.Forms.LinkLabel();
			this.tblLayout.SuspendLayout();
			this.mnuTrayIcon.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnClose.Location = new System.Drawing.Point(271, 46);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 29);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// tblLayout
			// 
			this.tblLayout.ColumnCount = 2;
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
			this.tblLayout.Controls.Add(this.lnkGitHub, 0, 1);
			this.tblLayout.Controls.Add(this.btnClose, 1, 1);
			this.tblLayout.Controls.Add(this.lblHostingDetail, 0, 0);
			this.tblLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblLayout.Location = new System.Drawing.Point(0, 0);
			this.tblLayout.Name = "tblLayout";
			this.tblLayout.RowCount = 2;
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tblLayout.Size = new System.Drawing.Size(349, 78);
			this.tblLayout.TabIndex = 2;
			// 
			// lblHostingDetail
			// 
			this.lblHostingDetail.AutoSize = true;
			this.tblLayout.SetColumnSpan(this.lblHostingDetail, 2);
			this.lblHostingDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHostingDetail.Location = new System.Drawing.Point(3, 0);
			this.lblHostingDetail.Name = "lblHostingDetail";
			this.lblHostingDetail.Size = new System.Drawing.Size(343, 43);
			this.lblHostingDetail.TabIndex = 2;
			this.lblHostingDetail.TabStop = true;
			this.lblHostingDetail.Text = "lblHostingDetail";
			this.lblHostingDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblHostingDetail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblHostingDetail_LinkClicked);
			// 
			// icoSystemTray
			// 
			this.icoSystemTray.BalloonTipTitle = "Draw Ship";
			this.icoSystemTray.ContextMenuStrip = this.mnuTrayIcon;
			this.icoSystemTray.Icon = ((System.Drawing.Icon)(resources.GetObject("icoSystemTray.Icon")));
			this.icoSystemTray.Text = "Draw Ship";
			this.icoSystemTray.Visible = true;
			this.icoSystemTray.DoubleClick += new System.EventHandler(this.icoSystemTray_DoubleClick);
			// 
			// mnuTrayIcon
			// 
			this.mnuTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmShowDetail,
            this.itmExit});
			this.mnuTrayIcon.Name = "mnuTrayIcon";
			this.mnuTrayIcon.Size = new System.Drawing.Size(137, 48);
			// 
			// itmShowDetail
			// 
			this.itmShowDetail.Name = "itmShowDetail";
			this.itmShowDetail.Size = new System.Drawing.Size(136, 22);
			this.itmShowDetail.Text = "Show Detail";
			this.itmShowDetail.Click += new System.EventHandler(this.icoSystemTray_DoubleClick);
			// 
			// itmExit
			// 
			this.itmExit.Name = "itmExit";
			this.itmExit.Size = new System.Drawing.Size(136, 22);
			this.itmExit.Text = "Exit";
			this.itmExit.Click += new System.EventHandler(this.itmExit_Click);
			// 
			// lnkGitHub
			// 
			this.lnkGitHub.AutoSize = true;
			this.lnkGitHub.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lnkGitHub.Location = new System.Drawing.Point(3, 43);
			this.lnkGitHub.Name = "lnkGitHub";
			this.lnkGitHub.Size = new System.Drawing.Size(259, 35);
			this.lnkGitHub.TabIndex = 3;
			this.lnkGitHub.TabStop = true;
			this.lnkGitHub.Text = "https://github.com/laingsimon/draw-ship#drawship";
			this.lnkGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lnkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGitHub_LinkClicked);
			// 
			// HostingDetail
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(349, 78);
			this.Controls.Add(this.tblLayout);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HostingDetail";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DrawShip hosting detail";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HostingDetail_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Shown += new System.EventHandler(this.HostingDetail_Shown);
			this.tblLayout.ResumeLayout(false);
			this.tblLayout.PerformLayout();
			this.mnuTrayIcon.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private System.Windows.Forms.LinkLabel lblHostingDetail;
		private System.Windows.Forms.NotifyIcon icoSystemTray;
		private System.Windows.Forms.ContextMenuStrip mnuTrayIcon;
		private System.Windows.Forms.ToolStripMenuItem itmShowDetail;
		private System.Windows.Forms.ToolStripMenuItem itmExit;
		private System.Windows.Forms.LinkLabel lnkGitHub;
	}
}

