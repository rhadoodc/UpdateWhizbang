using System;
using System.IO;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	static class Program
	{
		public static ConfigurationManager configManager;
		private static MainForm mainForm;

		public static string appDataPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Properties.Resources.sprocketAppDataFolder_key, Properties.Resources.updateMonitorAppDataFolder_key);
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetCompatibleTextRenderingDefault(false);
			Application.EnableVisualStyles();
			
			configManager = new ConfigurationManager();

			mainForm = new MainForm();

			configManager.Initialize();
			mainForm.Initialize();

			Application.Run(mainForm);
		}
	}
}
