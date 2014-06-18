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
	public partial class LogView : Form
	{
		private FileSystemWatcher fileSystemWatcher;

		private delegate void UpdateLogDelegate();
		private event UpdateLogDelegate updateLogDelegate;

		public LogView()
		{
			InitializeComponent();

			updateLogDelegate = Refresh;

			Refresh();

			fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(Program.currentLogFile), Path.GetFileName(Program.currentLogFile));

			fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.LastWrite | NotifyFilters.Size;

			fileSystemWatcher.Changed += fileSystemWatcher_Changed;

			fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void Refresh()
		{
			using (StreamReader reader = new StreamReader(Program.currentLogFile))
			{
				logTextBox.Text = reader.ReadToEnd();
			}
		}

		void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			Invoke(updateLogDelegate);
		}

		private void LogView_FormClosing(object sender, FormClosingEventArgs e)
		{
			fileSystemWatcher.Dispose();
		}
	}
}
