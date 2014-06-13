using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Sprocket.UpdateMonitor
{
	public class ConfigurationManager
	{
		const string configBucketLoadErrorTitle_key = "Couldn't load configurations...";
		const string configBucketLoadError_key = "An error occured trying to load '{0}':\n{1}";

		const string configBucketSaveErrorTitle_key = "Couldn't save configurations...";
		const string configBucketSaveError_key = "An error occured trying to save '{0}':\n{1}";

		const string configBucketChangedTitle_key = "Configurations changed...";
		const string configBucketChanged_key = "The configurations have been modified. Would you like to save the changes?";

		const string addConfigurationList_key = "[+] New Configuration [...]";

		const string configBucketFile_key = "configurations.xml";

		string ConfigBucketPath
		{
			get
			{
				return Path.Combine(Program.appDataPath, configBucketFile_key);
			}
		}

		public bool Modified = false;

		private Configuration activeConfig;

		public Configuration ActiveConfig
		{
			get
			{
				return activeConfig;
			}
		}

		List<Configuration> configBucket;

		XmlSerializer xmlSerializer;

		public delegate void ConfigListChangedDelegate(string[] configs);

		public event ConfigListChangedDelegate OnConfigListChanged;

		public event SyncManager.SyncListUpdatedDelegate OnSyncListUpdated;

		public delegate void CreateConfigRequestDelegate();

		public event CreateConfigRequestDelegate OnRequireNewConfig;

		public ConfigurationManager()
		{
			try
			{
				xmlSerializer = new XmlSerializer(typeof(List<Configuration>));
			}
			catch (System.Exception ex)
			{

			}
		}

		public void Initialize()
		{
			TryLoadConfigBucket();
		}

		private void TryLoadConfigBucket()
		{
			TextReader reader = null;
			try
			{
				reader = new StreamReader(ConfigBucketPath);
				configBucket = (List<Configuration>)xmlSerializer.Deserialize(reader);
			}
			catch (System.Exception ex)
			{
				configBucket = new List<Configuration>();

				if (!(ex is FileNotFoundException || ex is DirectoryNotFoundException))
				{
					MessageBox.Show(string.Format(configBucketLoadError_key, Path.GetFullPath(ConfigBucketPath), ex.Message),
								configBucketLoadErrorTitle_key,
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);
				}
			}
			finally
			{
				if (reader != null)
					reader.Close();

				Modified = false;

				UpdateConfigurationList();
			}

			if (configBucket.Count <= 0)
			{
				var eh = OnRequireNewConfig;

				if (eh != null)
				{
					eh();
				}
			}

			UpdateConfigurationList();
		}

		private void SaveConfigBucket()
		{
			TextWriter writer = null;

			var oldModified = Modified;

			Modified = false;

			try
			{
				var s = Path.GetFullPath(ConfigBucketPath).Replace(Path.GetFileName(ConfigBucketPath), string.Empty);
				Directory.CreateDirectory(s);
				writer = new StreamWriter(ConfigBucketPath);
				xmlSerializer.Serialize(writer, configBucket);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(string.Format(configBucketSaveError_key, Path.GetFullPath(ConfigBucketPath), ex.Message),
								configBucketSaveErrorTitle_key,
								MessageBoxButtons.OK,
								MessageBoxIcon.Error,
								MessageBoxDefaultButton.Button1);

				Modified = oldModified;
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}

		public DialogResult AskSaveChanges()
		{
			var modified = false;

			foreach (var config in configBucket)
			{
				modified |= config.syncManager.Modified;
			}

			if (Modified || modified)
			{
				var userAnswer = MessageBox.Show(configBucketChanged_key, 
												configBucketChangedTitle_key,
												MessageBoxButtons.YesNoCancel, 
												MessageBoxIcon.Warning);

				if (userAnswer == DialogResult.Yes)
				{
					SaveAll();
				}

				return userAnswer;
			}

			return DialogResult.None;
		}

		public void SaveAll()
		{
			SaveConfigBucket();

			foreach (var config in configBucket)
			{
				config.SyncManagerSave();
			}
		}

		public IEnumerable<SyncItem> GetSyncItems(List<string> itemKeys)
		{
			return activeConfig.syncManager.GetSyncItems(itemKeys);
		}

		public void AddSyncItem(SyncItem item)
		{
			activeConfig.syncManager.AddSyncItem(item);
		}

		public int SyncItemCount
		{
			get
			{
				if ((activeConfig == null) || (activeConfig.syncManager == null))
					return -1;

				return activeConfig.syncManager.SyncItemCount;
			}
		}

		public void RemoveSyncItem(string key)
		{
			activeConfig.syncManager.RemoveSyncItem(key);
		}

		public void AddConfiguration(string name)
		{
			configBucket.Add(new Configuration(name));

			Modified = true;

			UpdateConfigurationList();
		}

		public SyncItem GetItemByKey(string key)
		{
			return activeConfig.syncManager.GetItemByKey(key);
		}

		public void RemoveConfiguration(string name)
		{
			Configuration conf = null;
			foreach (var config in configBucket)
			{
				if (config.Name == name)
				{
					conf = config;
					break;
				}
			}

			if (conf != null)
				configBucket.Remove(conf);

			Modified = true;

			UpdateConfigurationList();
		}

		private void UpdateConfigurationList()
		{
			List<string> configs = new List<string>();

			foreach (var config in configBucket)
			{
				configs.Add(config.Name);
			}

			configs.Add(addConfigurationList_key);

			var eh = OnConfigListChanged;

			if (eh != null)
			{
				eh(configs.ToArray());
			}
		}

		public void SetActiveConfiguration(string activeConfigurationName)
		{
			AskSaveChanges();

			if (activeConfig != null)
				activeConfig.syncManager.OnSyncListUpdated -= OnSyncListUpdated;

			activeConfig = configBucket.Where(c => c.Name == activeConfigurationName).SingleOrDefault();

			activeConfig.syncManager.OnSyncListUpdated += OnSyncListUpdated;

			activeConfig.SyncManagerLoad();
			activeConfig.syncManager.UpdateSyncList();
		}
	}
}
