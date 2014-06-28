using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	public partial class MainForm : Form
	{
		const string confirmRemoveConfigTitle_key = "Removing configuration...";
		const string confirmRemoveConfig_key = "You are about to remove the configuration '{0}'. Are you sure?";

		const string multipleItemsSelected_key = "Multiple Items Selected [{0}]";
		const string multipleSourcePaths_key = "Multiple Source Paths";
		const string multipleTargetPaths_key = "Multiple Unmatched Target Paths";

		const string multipleUnmatchedTargetPathsOverride_key  = "The items selected have several paths that did not match before the edit operations performed. Proceeding with applying changes will assign to them only the paths that are common to all.";

		const string multipleUnmatchTargetPathsOverrideTitle_key = "Override Multiple Unmatched Paths";

		const string minimizedNotification_key = "The Whizbang has retreated into the shadows and will continue to do your bidding quietly. You can banish it completely by right-clicking its icon in the notification area.";
		const string minimized_key = "Minimized";

		const string syncAll_key = "Sync All";
		const string quit_key = "Quit";
		const string separator_key = "-";
		const string forceSyncSelected_key = "Force Sync Selected";
		const string delete_key = "Delete";
		const string properties_key = "Properties";

		const string syncingSelected_key = "Syncing Selected...";
		const string syncing_key = "Syncing...";
		const string synchronized_key = "Synchronized";
		const string updated_key = "Updated";

		const string synchronizedStatus_key = "The Whizbang has synchronized {0} items.";

		const string idle_key = "Idle";

		const string monitoredItemToolTip_key = "{0}\n{1} {2}";

		int lastSelectedConfigurationIndex = -1;

		bool showMinimizedNotification = true;

		Timer timer;
		//Timer dropBoxHintTimer;

		Timer newsTimer;

		public MainForm()
		{
			InitializeComponent();

			appVersionLabel.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
#if DEBUG
				+ "[DEBUG]"
#endif
				;

			Program.configManager.OnConfigListChanged += OnConfigurationListChanged;
			Program.configManager.OnSyncListUpdated += OnSyncListUpdated;
			Program.configManager.OnRequireNewConfig += OnRequireNewConfig;

			//timer = new Timer();
			//timer.Enabled = true;
			//timer.Interval = 10000;
			//timer.Tick += Sync;

			newsTimer = new Timer();
			newsTimer.Enabled = true;
			newsTimer.Interval = 10000;
			newsTimer.Tick += ShowSyncItemChanges;

			//dropBoxHintTimer = new Timer();
			//dropBoxHintTimer.Enabled = false;
			//dropBoxHintTimer.Interval = 1000;
			//dropBoxHintTimer.Tick += HideDropboxHint;

			//dropboxHint_desaturated.Parent = monitoredFilesListView;

			systrayContextMenu.Items.Add(syncAll_key, Properties.Resources.forceSync, Sync);
			systrayContextMenu.Items.Add(separator_key);
			systrayContextMenu.Items.Add(quit_key, Properties.Resources.stop, Quit);
		}

		//private void ShowDropboxHint()
		//{
		//	dropboxHint_desaturated.Visible = true;

		//	dropBoxHintTimer.Enabled = true;
		//	dropBoxHintTimer.Start();
		//}

		//private void HideDropboxHint(object sender, EventArgs e)
		//{
		//	dropboxHint_desaturated.Visible = false;

		//	dropBoxHintTimer.Stop();
		//	dropBoxHintTimer.Enabled = false;
		//}

		private void Sync(object sender, EventArgs e)
		{
			SyncAll();
		}

		private void Quit(object sender, EventArgs e)
		{
			var answer = Program.configManager.AskSaveChanges();

			Properties.Settings.Default.LastUsedConfiguration = activeConfigurationComboBox.SelectedIndex;
			Properties.Settings.Default.Save();

			if (answer != System.Windows.Forms.DialogResult.Cancel)
			{
				//if (timer != null)
					//timer.Dispose();

				if (newsTimer != null)
					newsTimer.Dispose();

				notifyIcon.Dispose();

				Application.Exit();
				Environment.Exit(0);
			}
		}

		private void OnRequireNewConfig()
		{
			var addConfig = new AddConfiguration();

			var result = addConfig.ShowDialog();

			if (result == System.Windows.Forms.DialogResult.OK)
			{
				Program.configManager.AddConfiguration(addConfig.configNameTextBox.Text);

				activeConfigurationComboBox.SelectedIndex = activeConfigurationComboBox.Items.Count - 2;
			}
			else if (result == System.Windows.Forms.DialogResult.Cancel)
			{
				Application.Exit();
			}
		}

		private void addConfigurationButton_Click(object sender, EventArgs e)
		{
			var addConfig = new AddConfiguration();

			var result = addConfig.ShowDialog();

			if (result == System.Windows.Forms.DialogResult.OK)
			{
				Program.configManager.AddConfiguration(addConfig.configNameTextBox.Text);

				activeConfigurationComboBox.SelectedIndex = activeConfigurationComboBox.Items.Count - 2;
			}
		}

		private void removeConfigurationButton_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show(string.Format(confirmRemoveConfig_key, activeConfigurationComboBox.Text),
										confirmRemoveConfigTitle_key, 
										MessageBoxButtons.YesNo);

			Program.Log(string.Format(confirmRemoveConfig_key, activeConfigurationComboBox.Text), confirmRemoveConfigTitle_key, string.Format(Program.userResponse_key, result.ToString()));

			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				Program.configManager.RemoveConfiguration(activeConfigurationComboBox.Text);
			}
		}

		public void OnConfigurationListChanged(string[] configs)
		{
			activeConfigurationComboBox.Items.Clear();

			foreach (var config in configs)
			{
				activeConfigurationComboBox.Items.Add(config);
			}
		}

		private void OnSyncListUpdated(Dictionary<string, SyncItem> items, ImageList iconImageList)
		{
			monitoredFilesListView.SmallImageList = iconImageList;
			monitoredFilesListView.LargeImageList = iconImageList;

			//handle removed items as well

			foreach (var kvp in items)
			{
				if (!monitoredFilesListView.Items.ContainsKey(kvp.Key))
				{
					monitoredFilesListView.Items.Add(kvp.Key, kvp.Value.SourceFileInfo.Name, kvp.Value.SourceFileInfo.Extension);
					monitoredFilesListView.Items[kvp.Key].ToolTipText = string.Format(monitoredItemToolTip_key, kvp.Value.SourceFileInfo.Name, kvp.Value.SourceFileInfo.LastWriteTime.ToShortDateString(), kvp.Value.SourceFileInfo.LastWriteTime.ToShortTimeString());
				}
			}

			var toRemove = new List<ListViewItem>();

			foreach (ListViewItem item in monitoredFilesListView.Items)
			{
				if (!items.Values.Select(i => i.SourceFileInfo.Name).Contains(item.Text))
				{
					toRemove.Add(item);
				}
			}

			foreach (var rem in toRemove)
			{
				monitoredFilesListView.Items.Remove(rem);
			}

			if (Program.configManager.SyncItemCount <= 0)
			{
				dropboxHint.Visible = true;
			}
			else
			{
				dropboxHint.Visible = false;
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Program.configManager.OnConfigListChanged -= OnConfigurationListChanged;
			Program.configManager.OnSyncListUpdated -= OnSyncListUpdated;
		}

		private void activeConfigurationComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (activeConfigurationComboBox.SelectedIndex == activeConfigurationComboBox.Items.Count - 1)
			{
				var addConfig = new AddConfiguration();

				var result = addConfig.ShowDialog();

				if (result == System.Windows.Forms.DialogResult.OK)
				{
					Program.configManager.AddConfiguration(addConfig.configNameTextBox.Text);

					activeConfigurationComboBox.SelectedIndex = activeConfigurationComboBox.Items.Count - 2;
				}
				else if (result == System.Windows.Forms.DialogResult.Cancel)
				{
					activeConfigurationComboBox.SelectedIndex = lastSelectedConfigurationIndex;
				}
			}
			else
			{
				Program.configManager.SetActiveConfiguration(activeConfigurationComboBox.SelectedItem.ToString());

				SyncAll();

				lastSelectedConfigurationIndex = activeConfigurationComboBox.SelectedIndex;
			}
		}

		private void monitoredFilesListView_DragDrop(object sender, DragEventArgs ev)
		{
			if (ev.Effect == DragDropEffects.Copy)
			{
				var paths = (IEnumerable<string>)ev.Data.GetData(DataFormats.FileDrop);

				var createdItems = new List<SyncItem>();

				foreach (var path in paths)
				{
					var item = new SyncItem(path);

					createdItems.Add(item);
				}

				var result = OpenSyncItemProperties(createdItems);

				if (result == DialogResult.OK)
				{
					foreach (var item in createdItems)
					{
						Program.configManager.AddSyncItem(item);
					}
				}
			}
		}

		private void monitoredFilesListView_DragEnter(object sender, DragEventArgs ev)
		{
			if (ev.Data.GetDataPresent(DataFormats.FileDrop))
			{
				ev.Effect = DragDropEffects.Copy;
			}
			else
			{
				ev.Effect = DragDropEffects.None;
			}
		}

		private DialogResult OpenSyncItemProperties(IEnumerable<SyncItem> targetItems)
		{
			var props = new SyncItemProperties();
			var n = targetItems.Count();

			bool multipleUnmatchedTargets = false;

			if (n == 1)
			{
				var targetItem = targetItems.Single();
				
				props.selectedItemLabel.Text = targetItem.SourceFileInfo.Name;
				
				int i = 0;

				foreach (var targetPath in targetItem.targetPaths)
				{
					props.ConstructTargetField(i);
					props.targetPathTextBoxes[i].Text = targetPath;
					++i;
				}

				props.sourceTextBox.Text = targetItem.SourcePath;
			}
			else if (n > 0)
			{
				props.selectedItemLabel.Text = string.Format(multipleItemsSelected_key, n);

				props.sourceTextBox.Text = multipleSourcePaths_key;

				var targetList = new List<string>();

				int i = 0;

				foreach (var item in targetItems)
				{
					foreach (var path in item.targetPaths)
					{
						if (targetItems.All(it => it.targetPaths.Contains(path)))
						{
							if (!targetList.Contains(path))
							{
								targetList.Add(path);
								props.ConstructTargetField(i);
								props.targetPathTextBoxes[i].Text = path;
								++i;
							}
						}
						else
						{
							multipleUnmatchedTargets = true;
						}
					}
				}

				if (multipleUnmatchedTargets)
				{
					props.ConstructTargetField(i);
					props.targetPathTextBoxes[i].Text = multipleTargetPaths_key;
					props.targetPathTextBoxes[i].Enabled = false;
					++i;
				}
			}

			var result = props.ShowDialog();

			if (result == DialogResult.OK)
			{
				if (!multipleUnmatchedTargets)
				{
					foreach (var item in targetItems)
					{
						item.targetPaths = props.targetPathTextBoxes.Where(b => b.Enabled == true).Select(t => t.Text).Where(x => x != string.Empty).ToList();
					}
				}
				else
				{
					var userAnswer = MessageBox.Show(multipleUnmatchedTargetPathsOverride_key, multipleUnmatchTargetPathsOverrideTitle_key, MessageBoxButtons.OKCancel);

					Program.Log(multipleUnmatchedTargetPathsOverride_key, multipleUnmatchTargetPathsOverrideTitle_key, string.Format(Program.userResponse_key, result.ToString()));

					if (result == DialogResult.OK)
					{
						foreach (var item in targetItems)
						{
							item.targetPaths = props.targetPathTextBoxes.Where(b => b.Enabled == true).Select(t => t.Text).Where(x => x != string.Empty).ToList();
						}
					}
				}
			}

			return result;
		}

		private void listViewContextMenu_Opening(object sender, CancelEventArgs e)
		{
			listViewContextMenu.Items.Clear();

			e.Cancel = false;

			listViewContextMenu.Items.Add(forceSyncSelected_key, Properties.Resources.forceSync, forceSyncSelectedButton_Click);

			if (monitoredFilesListView.SelectedItems.Count > 0)
			{
				listViewContextMenu.Items.Add(separator_key);
				listViewContextMenu.Items.Add(delete_key, Properties.Resources.syncItemDelete, OnDeleteClicked);
				listViewContextMenu.Items.Add(separator_key);
				listViewContextMenu.Items.Add(properties_key, Properties.Resources.syncItemProperties, OnPropertiesClicked);
			}
		}

		private void OnDeleteClicked(object sender, EventArgs e)
		{
			foreach (ListViewItem item in monitoredFilesListView.SelectedItems)
			{
				Program.configManager.RemoveSyncItem(item.Name);
			}

			Program.configManager.ActiveConfig.syncManager.UpdateSyncList();
		}

		private void OnPropertiesClicked(object sender, EventArgs e)
		{
			var propList = new List<SyncItem>();

			foreach (ListViewItem item in monitoredFilesListView.SelectedItems)
			{
				propList.Add(Program.configManager.GetItemByKey(item.Name));
			}

			var result = OpenSyncItemProperties(propList);
		}

		private void forceSyncSelectedButton_Click(object sender, EventArgs e)
		{
			applicationStatusLabel.Text = syncingSelected_key;

			currentActionProgressBar.Visible = true;

			var n = monitoredFilesListView.SelectedItems.Count;

			currentActionProgressBar.Minimum = 0;
			currentActionProgressBar.Maximum = n;

			int syncCounter = 0;

			for (var i = 0; i < n; i++)
			{
				currentActionProgressBar.PerformStep();
				ListViewItem item = monitoredFilesListView.SelectedItems[i];
				var didWork = Program.configManager.GetItemByKey(item.Name).Sync();

				if (didWork)
					++syncCounter;
			}

			if (syncCounter > 0)
			{
				Program.Log(string.Format(synchronizedStatus_key, syncCounter));

				notifyIcon.ShowBalloonTip(2500, synchronized_key, string.Format(synchronizedStatus_key, syncCounter), ToolTipIcon.Info);
			}

			currentActionProgressBar.Visible = false;

			applicationStatusLabel.Text = idle_key;
		}

		private void SyncAll()
		{
			applicationStatusLabel.Text = syncing_key;

			currentActionProgressBar.Visible = true;

			var n = monitoredFilesListView.Items.Count;

			currentActionProgressBar.Minimum = 0;
			currentActionProgressBar.Maximum = n;

			currentActionProgressBar.Step = 1;

			int syncCounter = 0;

			for (var i = 0; i < n; i++)
			{
				currentActionProgressBar.PerformStep();
				ListViewItem item = monitoredFilesListView.Items[i];
				var didWork = Program.configManager.GetItemByKey(item.Name).Sync();

				if (didWork)
					++syncCounter;
			}

			if (syncCounter > 0)
			{
				Program.Log(string.Format(synchronizedStatus_key, syncCounter));

				notifyIcon.ShowBalloonTip(2500, synchronized_key, string.Format(synchronizedStatus_key, syncCounter), ToolTipIcon.Info);
			}

			currentActionProgressBar.Visible = false;

			applicationStatusLabel.Text = idle_key;
		}

		private const string syncItemSourceChanged_key = "'{0}' was changed and obediently synchronized by the Whizbang.\n";
		private const string syncItemSourceRenamed_key = "'{0}' was renamed to '{1}' and the Whizbang has taken note.\n";
		private const string syncItemSourceDeleted_key = "'{0}' was deleted or moved, but the Whizbang remembers it fondly.\n";
		private const string syncItemSourceError_key = "'{0}' has become unavailable, and the Whizbang will miss it.\n";

		private string syncItemSourceChanges = string.Empty;

		public void item_SyncItemSourceChanged(string itemPath)
		{
			syncItemSourceChanges += string.Format(syncItemSourceChanged_key, Path.GetFileName(itemPath));
		}

		public void item_SyncItemSourceRenamed(string itemOldPath, string itemNewPath)
		{
			syncItemSourceChanges += string.Format(syncItemSourceRenamed_key, Path.GetFileName(itemOldPath), Path.GetFileName(itemNewPath));
		}

		public void item_SyncItemSourceDeleted(string itemPath)
		{
			syncItemSourceChanges += string.Format(syncItemSourceDeleted_key, Path.GetFileName(itemPath));
		}

		public void item_SyncItemSourceError(string itemPath)
		{
			syncItemSourceChanges += string.Format(syncItemSourceError_key, Path.GetFileName(itemPath));
		}

		private void ShowSyncItemChanges(object sender, EventArgs e)
		{
			if (syncItemSourceChanges != string.Empty)
			{
				Program.Log(syncItemSourceChanges);

				notifyIcon.ShowBalloonTip(2500, updated_key, syncItemSourceChanges, ToolTipIcon.Info);

				syncItemSourceChanges = string.Empty;
			}
		}

		internal void Initialize()
		{
			activeConfigurationComboBox.SelectedIndex = Properties.Settings.Default.LastUsedConfiguration;
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			if (FormWindowState.Minimized == this.WindowState)
			{
				this.Hide();
				if (showMinimizedNotification)
				{
					showMinimizedNotification = false;
					notifyIcon.ShowBalloonTip(2500, minimized_key, minimizedNotification_key, ToolTipIcon.Info);
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Hide();
			e.Cancel = true;

			if (showMinimizedNotification)
			{
				showMinimizedNotification = false;
				notifyIcon.ShowBalloonTip(2500, minimized_key, minimizedNotification_key, ToolTipIcon.Info);
			}
		}

		private void systrayContextMenu_Opening(object sender, CancelEventArgs e)
		{
			e.Cancel = false;
		}

		private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.Show();
				this.WindowState = FormWindowState.Normal;
				this.Activate();
			}
		}

		private void MainForm_Activated(object sender, EventArgs e)
		{
			//if ((dropboxHint.Visible == false) && (Visible == true))
			//	ShowDropboxHint();
		}

		private void saveConfigurationsButton_Click(object sender, EventArgs e)
		{
			Program.configManager.SaveAll();
		}

		private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			var logView = new LogView();
			logView.Show();
		}

		private void showLogViewButton_Click(object sender, EventArgs e)
		{
			var logView = new LogView();
			logView.Show();
		}
	}
}
