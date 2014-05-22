using Sprocket.UpdateMonitor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	public partial class SyncItemProperties : Form
	{
		const string invalidTargetPath_key = "The target path is invalid: {0}";
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

		private void browseButton_Click(object sender, EventArgs e)
		{
			var result = folderBrowserDialog.ShowDialog();
			
			if (result == DialogResult.OK)
			{
				targetPathTextBox.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void targetPathTextBox_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				var path = Path.GetFullPath(targetPathTextBox.Text);
			}
			catch (System.Exception ex)
			{
				e.Cancel = true;
				applyButton.Enabled = false;

				errorProvider.Icon = Resources.warning_icon;
				errorProvider.SetIconPadding(targetPathTextBox, 5);
				errorProvider.SetError(targetPathTextBox, string.Format(invalidTargetPath_key, ex.Message));
			}
		}

		private void targetPathTextBox_TextChanged(object sender, EventArgs e)
		{
			this.Validate();
		}
	}
}
