using System;
using System.Xml.Serialization;

namespace Sprocket.UpdateMonitor
{
	[Serializable]
	public class Configuration
	{
		const string configLocation_key = "configs/{0}.xml";

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
