using System;
using System.IO;
using System.Xml.Serialization;

namespace Sprocket.UpdateMonitor
{
	[Serializable]
	public class Configuration
	{
		const string configFolder_key = "configs";
		const string configFile_key = "{0}.xml";
		string configLocation_key
		{
			get
			{
				return Path.Combine(Program.AppDataPath, configFolder_key, configFile_key);
			}
		}

		public string name;

		[NonSerialized]
		[XmlIgnore]
		public SyncManager syncManager = null;

		public string Name
		{
			get
			{
				return name;
			}
		}

		public Configuration()
		{
			name = string.Empty;

			syncManager = new SyncManager();
		}

		public Configuration (string _name)
		{
			name = _name;

			syncManager = new SyncManager();
		}

		public void SyncManagerLoad()
		{
			syncManager.LoadSyncList(string.Format(configLocation_key, name));
		}

		public void SyncManagerSave()
		{
			syncManager.SaveSyncList(string.Format(configLocation_key, name));
		}
	}
}
