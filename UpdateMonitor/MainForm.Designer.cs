namespace Sprocket.UpdateMonitor
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.activeConfigurationComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.addConfigurationButton = new System.Windows.Forms.ToolStripButton();
			this.removeConfigurationButton = new System.Windows.Forms.ToolStripButton();
			this.forceSyncSelectedButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.forceUpdateButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.monitoredFilesListView = new System.Windows.Forms.ListView();
			this.listViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
			this.applicationStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.currentActionProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.iconImageList = new System.Windows.Forms.ImageList(this.components);
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.systrayContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.dropboxHint = new System.Windows.Forms.PictureBox();
			this.dropboxHint_desaturated = new System.Windows.Forms.PictureBox();
			this.dropboxTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.toolStrip1.SuspendLayout();
			this.mainStatusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dropboxHint)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dropboxHint_desaturated)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activeConfigurationComboBox,
            this.addConfigurationButton,
            this.removeConfigurationButton,
            this.forceSyncSelectedButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(298, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "MainToolStrip";
			// 
			// activeConfigurationComboBox
			// 
			this.activeConfigurationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.activeConfigurationComboBox.DropDownWidth = 196;
			this.activeConfigurationComboBox.MaxLength = 64;
			this.activeConfigurationComboBox.Name = "activeConfigurationComboBox";
			this.activeConfigurationComboBox.Size = new System.Drawing.Size(121, 25);
			this.activeConfigurationComboBox.SelectedIndexChanged += new System.EventHandler(this.activeConfigurationComboBox_SelectedIndexChanged);
			// 
			// addConfigurationButton
			// 
			this.addConfigurationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.addConfigurationButton.Image = global::Sprocket.UpdateMonitor.Properties.Resources.add;
			this.addConfigurationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addConfigurationButton.Name = "addConfigurationButton";
			this.addConfigurationButton.Size = new System.Drawing.Size(23, 22);
			this.addConfigurationButton.Text = "Add Configuration";
			this.addConfigurationButton.Click += new System.EventHandler(this.addConfigurationButton_Click);
			// 
			// removeConfigurationButton
			// 
			this.removeConfigurationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.removeConfigurationButton.Image = global::Sprocket.UpdateMonitor.Properties.Resources.delete;
			this.removeConfigurationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.removeConfigurationButton.Name = "removeConfigurationButton";
			this.removeConfigurationButton.Size = new System.Drawing.Size(23, 22);
			this.removeConfigurationButton.Text = "Remove Configuration";
			this.removeConfigurationButton.Click += new System.EventHandler(this.removeConfigurationButton_Click);
			// 
			// forceSyncSelectedButton
			// 
			this.forceSyncSelectedButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.forceSyncSelectedButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.forceSyncSelectedButton.Image = global::Sprocket.UpdateMonitor.Properties.Resources.forceSync;
			this.forceSyncSelectedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.forceSyncSelectedButton.Name = "forceSyncSelectedButton";
			this.forceSyncSelectedButton.Size = new System.Drawing.Size(23, 22);
			this.forceSyncSelectedButton.Text = "Force Sync Selected";
			this.forceSyncSelectedButton.Click += new System.EventHandler(this.forceSyncSelectedButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// forceUpdateButton
			// 
			this.forceUpdateButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.forceUpdateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.forceUpdateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.forceUpdateButton.Name = "forceUpdateButton";
			this.forceUpdateButton.Size = new System.Drawing.Size(23, 22);
			this.forceUpdateButton.Text = "Force Update";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// monitoredFilesListView
			// 
			this.monitoredFilesListView.AllowDrop = true;
			this.monitoredFilesListView.ContextMenuStrip = this.listViewContextMenu;
			this.monitoredFilesListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.monitoredFilesListView.HideSelection = false;
			this.monitoredFilesListView.Location = new System.Drawing.Point(0, 25);
			this.monitoredFilesListView.Name = "monitoredFilesListView";
			this.monitoredFilesListView.Size = new System.Drawing.Size(298, 256);
			this.monitoredFilesListView.TabIndex = 1;
			this.dropboxTooltip.SetToolTip(this.monitoredFilesListView, "Drag & Drop\r\n   Files Here");
			this.monitoredFilesListView.UseCompatibleStateImageBehavior = false;
			this.monitoredFilesListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.monitoredFilesListView_DragDrop);
			this.monitoredFilesListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.monitoredFilesListView_DragEnter);
			// 
			// listViewContextMenu
			// 
			this.listViewContextMenu.Name = "listViewContextMenu";
			this.listViewContextMenu.Size = new System.Drawing.Size(61, 4);
			this.listViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.listViewContextMenu_Opening);
			// 
			// mainStatusStrip
			// 
			this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationStatusLabel,
            this.currentActionProgressBar});
			this.mainStatusStrip.Location = new System.Drawing.Point(0, 259);
			this.mainStatusStrip.Name = "mainStatusStrip";
			this.mainStatusStrip.Size = new System.Drawing.Size(298, 22);
			this.mainStatusStrip.TabIndex = 2;
			this.mainStatusStrip.Text = "statusStrip1";
			// 
			// applicationStatusLabel
			// 
			this.applicationStatusLabel.Name = "applicationStatusLabel";
			this.applicationStatusLabel.Size = new System.Drawing.Size(26, 17);
			this.applicationStatusLabel.Text = "Idle";
			// 
			// currentActionProgressBar
			// 
			this.currentActionProgressBar.Name = "currentActionProgressBar";
			this.currentActionProgressBar.Size = new System.Drawing.Size(100, 16);
			this.currentActionProgressBar.Visible = false;
			// 
			// iconImageList
			// 
			this.iconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.iconImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.iconImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.systrayContextMenu;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "Update Whizbang";
			this.notifyIcon.Visible = true;
			this.notifyIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseUp);
			// 
			// systrayContextMenu
			// 
			this.systrayContextMenu.Name = "systrayContextMenu";
			this.systrayContextMenu.Size = new System.Drawing.Size(61, 4);
			this.systrayContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.systrayContextMenu_Opening);
			// 
			// dropboxHint
			// 
			this.dropboxHint.BackgroundImage = global::Sprocket.UpdateMonitor.Properties.Resources.dropbox;
			this.dropboxHint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.dropboxHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dropboxHint.Enabled = false;
			this.dropboxHint.Location = new System.Drawing.Point(0, 25);
			this.dropboxHint.Name = "dropboxHint";
			this.dropboxHint.Size = new System.Drawing.Size(298, 234);
			this.dropboxHint.TabIndex = 4;
			this.dropboxHint.TabStop = false;
			this.dropboxHint.Visible = false;
			// 
			// dropboxHint_desaturated
			// 
			this.dropboxHint_desaturated.BackColor = System.Drawing.Color.Transparent;
			this.dropboxHint_desaturated.BackgroundImage = global::Sprocket.UpdateMonitor.Properties.Resources.dropbox_desaturated;
			this.dropboxHint_desaturated.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.dropboxHint_desaturated.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dropboxHint_desaturated.Enabled = false;
			this.dropboxHint_desaturated.Location = new System.Drawing.Point(0, 25);
			this.dropboxHint_desaturated.Name = "dropboxHint_desaturated";
			this.dropboxHint_desaturated.Size = new System.Drawing.Size(298, 234);
			this.dropboxHint_desaturated.TabIndex = 5;
			this.dropboxHint_desaturated.TabStop = false;
			this.dropboxHint_desaturated.Visible = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(298, 281);
			this.Controls.Add(this.dropboxHint_desaturated);
			this.Controls.Add(this.dropboxHint);
			this.Controls.Add(this.mainStatusStrip);
			this.Controls.Add(this.monitoredFilesListView);
			this.Controls.Add(this.toolStrip1);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.ShowInTaskbar = false;
			this.Text = "Update Whizbang";
			this.Activated += new System.EventHandler(this.MainForm_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.mainStatusStrip.ResumeLayout(false);
			this.mainStatusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dropboxHint)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dropboxHint_desaturated)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripComboBox activeConfigurationComboBox;
		private System.Windows.Forms.ToolStripButton addConfigurationButton;
		private System.Windows.Forms.ToolStripButton removeConfigurationButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ListView monitoredFilesListView;
		private System.Windows.Forms.ToolStripButton forceUpdateButton;
		private System.Windows.Forms.StatusStrip mainStatusStrip;
		private System.Windows.Forms.ContextMenuStrip listViewContextMenu;
		public System.Windows.Forms.ImageList iconImageList;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton forceSyncSelectedButton;
		public System.Windows.Forms.ToolStripStatusLabel applicationStatusLabel;
		public System.Windows.Forms.ToolStripProgressBar currentActionProgressBar;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip systrayContextMenu;
		private System.Windows.Forms.PictureBox dropboxHint;
		private System.Windows.Forms.PictureBox dropboxHint_desaturated;
		private System.Windows.Forms.ToolTip dropboxTooltip;
	}
}

