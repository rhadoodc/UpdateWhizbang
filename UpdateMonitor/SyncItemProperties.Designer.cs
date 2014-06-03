namespace Sprocket.UpdateMonitor
{
	partial class SyncItemProperties
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
			this.selectedItemLabel = new System.Windows.Forms.Label();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.applyButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.sourceTextBox = new System.Windows.Forms.TextBox();
			this.sourceLabel = new System.Windows.Forms.Label();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.addTargetButton = new System.Windows.Forms.Button();
			this.bottomControlsPannel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.bottomControlsPannel.SuspendLayout();
			this.SuspendLayout();
			// 
			// selectedItemLabel
			// 
			this.selectedItemLabel.AutoSize = true;
			this.selectedItemLabel.Location = new System.Drawing.Point(12, 9);
			this.selectedItemLabel.Name = "selectedItemLabel";
			this.selectedItemLabel.Size = new System.Drawing.Size(67, 13);
			this.selectedItemLabel.TabIndex = 0;
			this.selectedItemLabel.Text = "selectedItem";
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.applyButton.Location = new System.Drawing.Point(233, 4);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(50, 23);
			this.applyButton.TabIndex = 3;
			this.applyButton.Text = "Apply";
			this.applyButton.UseVisualStyleBackColor = true;
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(289, 4);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(50, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// sourceTextBox
			// 
			this.sourceTextBox.Enabled = false;
			this.sourceTextBox.Location = new System.Drawing.Point(56, 42);
			this.sourceTextBox.Name = "sourceTextBox";
			this.sourceTextBox.Size = new System.Drawing.Size(276, 20);
			this.sourceTextBox.TabIndex = 6;
			// 
			// sourceLabel
			// 
			this.sourceLabel.AutoSize = true;
			this.sourceLabel.Location = new System.Drawing.Point(12, 45);
			this.sourceLabel.Name = "sourceLabel";
			this.sourceLabel.Size = new System.Drawing.Size(41, 13);
			this.sourceLabel.TabIndex = 7;
			this.sourceLabel.Text = "Source";
			// 
			// errorProvider
			// 
			this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			this.errorProvider.RightToLeft = true;
			// 
			// addTargetButton
			// 
			this.addTargetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addTargetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.addTargetButton.Image = global::Sprocket.UpdateMonitor.Properties.Resources.add;
			this.addTargetButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.addTargetButton.Location = new System.Drawing.Point(3, 4);
			this.addTargetButton.Name = "addTargetButton";
			this.addTargetButton.Size = new System.Drawing.Size(84, 23);
			this.addTargetButton.TabIndex = 8;
			this.addTargetButton.Text = "Add Target";
			this.addTargetButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.addTargetButton.UseVisualStyleBackColor = true;
			this.addTargetButton.Click += new System.EventHandler(this.addTargetButton_Click);
			// 
			// bottomControlsPannel
			// 
			this.bottomControlsPannel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bottomControlsPannel.Controls.Add(this.addTargetButton);
			this.bottomControlsPannel.Controls.Add(this.applyButton);
			this.bottomControlsPannel.Controls.Add(this.cancelButton);
			this.bottomControlsPannel.Location = new System.Drawing.Point(1, 67);
			this.bottomControlsPannel.Margin = new System.Windows.Forms.Padding(0);
			this.bottomControlsPannel.Name = "bottomControlsPannel";
			this.bottomControlsPannel.Size = new System.Drawing.Size(342, 44);
			this.bottomControlsPannel.TabIndex = 9;
			// 
			// SyncItemProperties
			// 
			this.AcceptButton = this.applyButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(343, 111);
			this.Controls.Add(this.bottomControlsPannel);
			this.Controls.Add(this.sourceLabel);
			this.Controls.Add(this.sourceTextBox);
			this.Controls.Add(this.selectedItemLabel);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SyncItemProperties";
			this.ShowInTaskbar = false;
			this.Text = "Sync Item Properties";
			this.Shown += new System.EventHandler(this.SyncItemProperties_Shown);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.bottomControlsPannel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button cancelButton;
		public System.Windows.Forms.Label selectedItemLabel;
		private System.Windows.Forms.Label sourceLabel;
		public System.Windows.Forms.TextBox sourceTextBox;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Button addTargetButton;
		private System.Windows.Forms.Panel bottomControlsPannel;
	}
}