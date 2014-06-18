using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Sprocket.UpdateMonitor
{
	public class SyncManager
	{
		private Dictionary<string, SyncItem> items;

		public ImageList iconImageList = new ImageList();

		public delegate void SyncListUpdatedDelegate(Dictionary<string, SyncItem> items, ImageList iconImageList);

		public event SyncListUpdatedDelegate OnSyncListUpdated;

		XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SyncItem>));
		const string syncItemsSaveError_key = "An error occured trying to save sync items to '{0}':\n{1}";
		const string syncItemsSaveErrorTitle_key = "Couldn't save sync items...";

		const string syncItemsLoadError_key = "An error occured trying to load sync items from '{0}':\n{1}";
		const string syncItemsLoadErrorTitle_key = "Couldn't load sync items...";

		const string syncItemAddedHeader_key = "Sync item added:";
		const string syncItemRemovedHeader_key = "Sync item removed:";

		public bool Modified = false;

		public int SyncItemCount
		{
			get
			{
				return items.Count;
			}
		}

		public SyncManager()
		{
			items = new Dictionary<string,SyncItem>();

			updateSyncListDelegate = UpdateSyncList;
		}

		public void LoadSyncList(string path)
		{
			TextReader reader = null;

			if (items == null)
				items = new Dictionary<string, SyncItem>();
			else
				items.Clear();

			try
			{
				reader = new StreamReader(path);

				var loadedList = (List<SyncItem>)xmlSerializer.Deserialize(reader);

				foreach (var item in loadedList)
				{
					items.Add((item.SourceFileInfo.FullName), item);

					item.SyncItemSourceChanged += Program.mainForm.item_SyncItemSourceChanged;
					item.SyncItemSourceRenamed += Program.mainForm.item_SyncItemSourceRenamed;
					item.SyncItemSourceDeleted += Program.mainForm.item_SyncItemSourceDeleted;
					item.SyncItemSourceError += Program.mainForm.item_SyncItemSourceError;

					item.SyncItemSourceRenamed += item_SyncItemSourceRenamed;
				}
			}
			catch (System.Exception ex)
			{
				if (!(ex is FileNotFoundException || ex is DirectoryNotFoundException))
				{
					Program.Log(string.Format(syncItemsLoadError_key, Path.GetFullPath(path), ex.Message),
								syncItemsLoadErrorTitle_key);

					MessageBox.Show(string.Format(syncItemsLoadError_key, Path.GetFullPath(path), ex.Message),
								syncItemsLoadErrorTitle_key,
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

				UpdateSyncList();
			}
		}

		public void SaveSyncList(string path)
		{
			if (!Modified)
				return;

			var oldModified = Modified;

			Modified = false;

			List<SyncItem> saveList = items.Values.ToList();

			TextWriter writer = null;

			try
			{
				var s = Path.GetFullPath(path).Replace(Path.GetFileName(path), string.Empty);
				Directory.CreateDirectory(s);
				writer = new StreamWriter(path);
				xmlSerializer.Serialize(writer, saveList);
			}
			catch (System.Exception ex)
			{
				Program.Log(string.Format(syncItemsSaveError_key, Path.GetFullPath(path), ex.Message),
								syncItemsSaveErrorTitle_key);

				MessageBox.Show(string.Format(syncItemsSaveError_key, Path.GetFullPath(path), ex.Message),
								syncItemsSaveErrorTitle_key,
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

		public void AddSyncItem (SyncItem item)
		{
			if (!items.ContainsKey(item.SourceFileInfo.FullName))
			{
				item.SyncItemSourceChanged += Program.mainForm.item_SyncItemSourceChanged;
				item.SyncItemSourceRenamed += Program.mainForm.item_SyncItemSourceRenamed;
				item.SyncItemSourceDeleted += Program.mainForm.item_SyncItemSourceDeleted;
				item.SyncItemSourceError += Program.mainForm.item_SyncItemSourceError;

				item.SyncItemSourceRenamed += item_SyncItemSourceRenamed;

				items.Add((item.SourceFileInfo.FullName), item); //hash this shorter?

				Program.Log(item.SourceFileInfo.FullName, syncItemAddedHeader_key);
			}

			Modified = true;

			UpdateSyncList();
		}

		private delegate void UpdateSyncListDelegate();
		private event UpdateSyncListDelegate updateSyncListDelegate;

		void item_SyncItemSourceRenamed(string itemOldPath, string itemNewPath)
		{
			var item = items[itemOldPath];
			items.Remove(itemOldPath);
			items.Add((item.SourceFileInfo.FullName), item); //hash this shorter?

			Program.mainForm.Invoke(updateSyncListDelegate);
		}

		public void RemoveSyncItem (string key)
		{
			Program.Log(items[key].SourceFileInfo.FullName, syncItemRemovedHeader_key);

			items.Remove(key);

			Modified = true;
		}

		public void UpdateSyncList()
		{
			foreach (var kvp in items)
			{
				ListViewItem item = new ListViewItem();

				item.Text = Path.GetFileName(kvp.Value.SourcePath);
				item.Name = kvp.Key;

				EnsureIcon(kvp.Value);

				item.ImageKey = kvp.Value.SourceFileInfo.Extension;
			}

			var eh = OnSyncListUpdated;

			if (eh != null)
			{
				eh(items, iconImageList);
			}
		}

		public void EnsureIcon(SyncItem source)
		{
			var file = source.SourceFileInfo;

			if (!iconImageList.Images.ContainsKey(file.Extension))
			{
				var fileIcon = IconExtractor.GetIconForFile(file.FullName, IconExtractor.ShellIconSize.ExtraLarge);
				iconImageList.Images.Add(file.Extension, fileIcon);
			}
		}

		internal IEnumerable<SyncItem> GetSyncItems(List<string> itemKeys)
		{
			return items.Where(kvp => itemKeys.Contains(kvp.Key)).Select(k => k.Value).ToArray();
		}

		public SyncItem GetItemByKey(string key)
		{
			SyncItem ret = null;
			if (items.TryGetValue(key, out ret))
			{
				return ret;
			}

			return null;
		}
	}
}
