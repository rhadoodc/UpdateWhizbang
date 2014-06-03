using Sprocket.UpdateMonitor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	public partial class SyncItemProperties : Form
	{
		private List<Button> browseButtons = new List<Button>();
		private List<Label> targetLabels = new List<Label>();

		public List<TextBox> targetPathTextBoxes = new List<TextBox>();

		const string invalidTargetPath_key = "The target path is invalid: {0}";

		const string targetPathLabel_key = "Target [{0}]";
		const string browseButton_key = "Browse [{0}]";

		const int controlLineSize = 20;
		const int controlLineMargin = 5;

		readonly Size browseButtonSize = new Size(50, controlLineSize);
		readonly Size targetTextBoxSize = new Size(200, controlLineSize);
		readonly Size targetLabelSize = new Size(40, controlLineSize);

		const int browseButtonXOffset = 280;
		const int targetTextBoxXOffset = 70;
		const int targetLabelXOffset = 10;

		const int targetLayoutBaseline = 67;

		public SyncItemProperties()
		{
			InitializeComponent();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;

			Close();
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;

			Close();
		}

		private void targetPathTextBox_TextChanged(object sender, EventArgs e)
		{
			this.Validate();
		}

		public void ConstructTargetField(int index)
		{
			if (index < 0)
			{
				index = targetPathTextBoxes.Count;
			}

			//target path text box
			var targetPathTextBox = new TextBox();

			targetPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));

			targetPathTextBox.Size = targetTextBoxSize;
			targetPathTextBox.Location = new Point(targetTextBoxXOffset, targetLayoutBaseline + (controlLineSize + controlLineMargin) * index);

			targetPathTextBox.TextChanged += targetPathTextBox_TextChanged;

			targetPathTextBox.Validating += (object sender, CancelEventArgs e) =>
			{
				try
				{
					var path = Path.GetFullPath(targetPathTextBox.Text);
				}
				catch (System.Exception ex)
				{
					e.Cancel = true;

					if (applyButton.Enabled == true)
					{
						applyButton.Enabled = false;

						errorProvider.Icon = Resources.warning_icon;
						errorProvider.SetIconPadding(targetPathTextBox, 5);
						errorProvider.SetError(targetPathTextBox, string.Format(invalidTargetPath_key, ex.Message));
					}
				}
			};

			targetPathTextBoxes.Insert(index, targetPathTextBox);

			Controls.Add(targetPathTextBox);

			//target label
			var targetLabel = new Label();

			targetLabel.Text = string.Format(targetPathLabel_key, index);

			targetLabel.Size = targetLabelSize;
			targetLabel.Location = new Point(targetLabelXOffset, targetLayoutBaseline + 3 + (controlLineSize + controlLineMargin) * index);

			targetLabels.Insert(index, targetLabel);

			Controls.Add(targetLabel);

			//browse button
			var browseButton = new Button();

			browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));

			browseButton.Text = string.Format(browseButton_key, index);

			browseButton.Size = browseButtonSize;
			browseButton.Location = new Point(browseButtonXOffset, targetLayoutBaseline + +(controlLineSize + controlLineMargin) * index);

			browseButton.Click += (object sender, EventArgs e) => 
			{
				var result = folderBrowserDialog.ShowDialog();

				if (result == DialogResult.OK)
				{
					targetPathTextBox.Text = folderBrowserDialog.SelectedPath;
				}
			};

			browseButtons.Insert(index, browseButton);

			Controls.Add(browseButton);

			this.Size = new Size(this.Size.Width, this.Size.Height + controlLineMargin + controlLineSize);
		}

		private void addTargetButton_Click(object sender, EventArgs e)
		{
			ConstructTargetField(-1);
		}

		private void SyncItemProperties_Shown(object sender, EventArgs e)
		{
			if (targetPathTextBoxes.Count == 0)
			{
				ConstructTargetField(-1);
			}
		}
	}
}
