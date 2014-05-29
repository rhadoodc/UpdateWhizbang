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
			this.browseButton = new System.Windows.Forms.Button();
			this.targetPathTextBox = new System.Windows.Forms.TextBox();
			this.applyButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.targetLabel = new System.Windows.Forms.Label();
			this.sourceTextBox = new System.Windows.Forms.TextBox();
			this.sourceLabel = new System.Windows.Forms.Label();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
			// browseButton
			// 
			this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseButton.Location = new System.Drawing.Point(282, 75);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(50, 23);
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "Browse";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// targetPathTextBox
			// 
			this.targetPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.targetPathTextBox.Location = new System.Drawing.Point(77, 77);
			this.targetPathTextBox.Name = "targetPathTextBox";
			this.targetPathTextBox.Size = new System.Drawing.Size(200, 20);
			this.targetPathTextBox.TabIndex = 2;
			this.targetPathTextBox.TextChanged += new System.EventHandler(this.targetPathTextBox_TextChanged);
			this.targetPathTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.targetPathTextBox_Validating);
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.applyButton.Location = new System.Drawing.Point(226, 131);
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
			this.cancelButton.Location = new System.Drawing.Point(282, 131);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(50, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// targetLabel
			// 
			this.targetLabel.AutoSize = true;
			this.targetLabel.Location = new System.Drawing.Point(12, 80);
			this.targetLabel.Name = "targetLabel";
			this.targetLabel.Size = new System.Drawing.Size(38, 13);
			this.targetLabel.TabIndex = 5;
			this.targetLabel.Text = "Target";
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
			// SyncItemProperties
			// 
			this.AcceptButton = this.applyButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(344, 166);
			this.Controls.Add(this.sourceLabel);
			this.Controls.Add(this.sourceTextBox);
			this.Controls.Add(this.targetLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.targetPathTextBox);
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.selectedItemLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SyncItemProperties";
			this.Text = "Sync Item Properties";
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button cancelButton;
		public System.Windows.Forms.Label selectedItemLabel;
		public System.Windows.Forms.TextBox targetPathTextBox;
		private System.Windows.Forms.Label targetLabel;
		private System.Windows.Forms.Label sourceLabel;
		public System.Windows.Forms.TextBox sourceTextBox;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}