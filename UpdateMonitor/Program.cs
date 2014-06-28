using System;
using System.IO;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	static class Program
	{
		public static ConfigurationManager configManager;
		public static MainForm mainForm;

		public const string logFolder_key = "logs";
		public const string logFileFormat_key = "{0} - {1}.txt";

		public const string userResponse_key = "User response: {0}\r\n";

		private const string timeStamp_key = "[{0}]";
		private const string newLine_key = "\r\n";

		private const string startingUpLog_key = "Starting up...";

		public static string LogFilesPath
		{
			get
			{
				return Path.Combine(Program.AppDataPath, logFolder_key);
			}
		}

		public static readonly string currentLogFile;

		public static string AppDataPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Properties.Resources.sprocketAppDataFolder_key, Properties.Resources.updateMonitorAppDataFolder_key);
			}
		}

		static Program()
		{
			var logFileName = string.Format(logFileFormat_key, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
			logFileName = logFileName.Replace('/', '-');
			logFileName = logFileName.Replace(':', '_');
			currentLogFile = Path.Combine(LogFilesPath, logFileName);
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetCompatibleTextRenderingDefault(false);
			Application.EnableVisualStyles();

			Directory.CreateDirectory(Path.GetDirectoryName(currentLogFile));
			
			configManager = new ConfigurationManager();

			mainForm = new MainForm();

			configManager.Initialize();
			mainForm.Initialize();

			Program.Log(startingUpLog_key);

			Application.Run(mainForm);
		}

		public static void Log(string contents, string header = "", string footer = "")
		{
			using (StreamWriter writer = new StreamWriter(Program.currentLogFile, true))
			{
				writer.WriteLine(timeStamp_key, DateTime.Now.ToLongTimeString());

				if (header != "")
					writer.WriteLine(header, DateTime.Now.ToLongTimeString());

				writer.Write(contents);

				if (footer != "")
					writer.WriteLine(footer);

				writer.WriteLine(newLine_key);
			}
		}

		public static bool IsFileLocked(FileInfo file)
		{
			FileStream stream = null;

			try
			{
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException ex)
			{
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				if (ex.GetType() == typeof(IOException))
					return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			//file is not locked
			return false;
		}
	}
}
