using System;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	static class Program
	{
		public static ConfigurationManager configManager;
		private static MainForm mainForm;

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
