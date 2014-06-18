using Sprocket.UpdateMonitor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	public partial class AddConfiguration : Form
	{
		const string invalidCharsError_key = "The configuration name can't contain any of the following characters: {0}.";
		const string nameTooShort_key = "The configuration name must be 3 characters or longer.";

		const string dialogShown_key = "User added configuration";

		const string listSeparator_key = ", ";
		const string lineSeparator_key = "\n";

		const string allGood_key = "All good!";

		private char[] invalidPathChars;

		List<string> errors = new List<string>();

		public AddConfiguration()
		{
			InitializeComponent();

			invalidPathChars = Path.GetInvalidFileNameChars();	
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;

			Close();
		}

		private void createButton_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;

			Close();
		}

		public new DialogResult ShowDialog()
		{
			base.ShowDialog();

			Program.Log(configNameTextBox.Text, dialogShown_key, string.Format(Program.userResponse_key, DialogResult.ToString()));

			return DialogResult;
		}

		private void configNameTextBox_Validating(object sender, CancelEventArgs e)
		{
			errors.Clear();

			if (configNameTextBox.Text.Length < 3)
			{
				errors.Add(nameTooShort_key);
			}

			if (configNameTextBox.Text.IndexOfAny(invalidPathChars) >= 0)
			{
				 errors.Add(string.Format(invalidCharsError_key, string.Join(listSeparator_key, invalidPathChars)));
			}

			if (errors.Count > 0)
			{
				e.Cancel = true;
				createButton.Enabled = false;

				configNameErrorProvider.Icon = Resources.warning_icon;
				configNameErrorProvider.SetIconPadding(configNameTextBox, 5);
				configNameErrorProvider.SetError(configNameTextBox, string.Join(lineSeparator_key, errors));
			}
		}

		private void configNameTextBox_Validated(object sender, EventArgs e)
		{
			createButton.Enabled = true;
			configNameErrorProvider.Icon = Resources.validated_icon;
			configNameErrorProvider.SetIconPadding(configNameTextBox, 5);
			configNameErrorProvider.SetError(configNameTextBox, allGood_key);

		}

		private void configNameTextBox_TextChanged(object sender, EventArgs e)
		{
			this.Validate();
		}
	}
}
