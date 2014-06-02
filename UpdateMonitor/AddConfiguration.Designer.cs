namespace Sprocket.UpdateMonitor
{
	partial class AddConfiguration
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddConfiguration));
			this.configNameTextBox = new System.Windows.Forms.TextBox();
			this.createButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.configNameErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.configNameLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.configNameErrorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// configNameTextBox
			// 
			this.configNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.configNameTextBox.Location = new System.Drawing.Point(25, 25);
			this.configNameTextBox.Margin = new System.Windows.Forms.Padding(5);
			this.configNameTextBox.MaxLength = 64;
			this.configNameTextBox.Name = "configNameTextBox";
			this.configNameTextBox.Size = new System.Drawing.Size(320, 20);
			this.configNameTextBox.TabIndex = 0;
			this.configNameTextBox.TextChanged += new System.EventHandler(this.configNameTextBox_TextChanged);
			this.configNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.configNameTextBox_Validating);
			this.configNameTextBox.Validated += new System.EventHandler(this.configNameTextBox_Validated);
			// 
			// createButton
			// 
			this.createButton.BackColor = System.Drawing.SystemColors.Control;
			this.createButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.createButton.CausesValidation = false;
			this.createButton.Enabled = false;
			this.createButton.ForeColor = System.Drawing.SystemColors.ControlText;
			this.createButton.Location = new System.Drawing.Point(14, 55);
			this.createButton.Margin = new System.Windows.Forms.Padding(5);
			this.createButton.Name = "createButton";
			this.createButton.Size = new System.Drawing.Size(75, 23);
			this.createButton.TabIndex = 1;
			this.createButton.Text = "Create";
			this.createButton.UseVisualStyleBackColor = false;
			this.createButton.Click += new System.EventHandler(this.createButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.BackColor = System.Drawing.SystemColors.Control;
			this.cancelButton.CausesValidation = false;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(265, 55);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(5);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = false;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// configNameErrorProvider
			// 
			this.configNameErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.configNameErrorProvider.ContainerControl = this;
			this.configNameErrorProvider.RightToLeft = true;
			// 
			// configNameLabel
			// 
			this.configNameLabel.AutoSize = true;
			this.configNameLabel.Location = new System.Drawing.Point(5, 5);
			this.configNameLabel.Name = "configNameLabel";
			this.configNameLabel.Size = new System.Drawing.Size(100, 13);
			this.configNameLabel.TabIndex = 3;
			this.configNameLabel.Text = "Configuration Name";
			// 
			// AddConfiguration
			// 
			this.AcceptButton = this.createButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(354, 87);
			this.Controls.Add(this.configNameLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.createButton);
			this.Controls.Add(this.configNameTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddConfiguration";
			this.Text = "Add Configuration";
			((System.ComponentModel.ISupportInitialize)(this.configNameErrorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button createButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ErrorProvider configNameErrorProvider;
		private System.Windows.Forms.Label configNameLabel;
		public System.Windows.Forms.TextBox configNameTextBox;
	}
}