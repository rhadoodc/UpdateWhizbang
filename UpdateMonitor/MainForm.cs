using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sprocket.UpdateMonitor
{
	public partial class MainForm : Form
	{
		const string confirmRemoveConfigTitle_key = "Removing configuration...";
		const string confirmRemoveConfig_key = "You are about to remove the configuration '{0}'. Are you sure?";

		const string multipleItemsSelected_key = "Multiple Items Selected [{0}]";
		const string multipleSourcePaths_key = "Multiple Source Paths";
		const string multipleTargetPaths_key = "Multiple Target Paths";

		const string minimizedNotification_key = "The whizbang has retreated into the shadows and will continue to do your bidding quietly. You can banish it completely by right-clicking its icon in the notification area.";
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

		const string synchronizedStatus_key = "The whizbang has synchronized {0} files.";

		const string idle_key = "Idle";

		int lastSelectedConfigurationIndex = -1;

		bool showMinimizedNotification = true;

		Timer timer;

		public MainForm()
		{
			InitializeComponent();

			Program.configManager.OnConfigListChanged += OnConfigurationListChanged;
			Program.configManager.OnSyncListUpdated += OnSyncListUpdated;
			Program.configManager.OnRequireNewConfig += OnRequireNewConfig;

			timer = new Timer();
			timer.Enabled = true;
			timer.Interval = 10000;
			timer.Tick += Sync;

			systrayContextMenu.Items.Add(syncAll_key, Properties.Resources.forceSync, Sync);
			systrayContextMenu.Items.Add(separator_key);
			systrayContextMenu.Items.Add(quit_key, Properties.Resources.stop, Quit);
		}

		private void Sync(object sender, EventArgs e)
		{
			SyncAll();
		}

		private void Quit(object sender, EventArgs e)
		{
			var answer = Program.configManager.AskSaveChanges();

			if (answer != System.Windows.Forms.DialogResult.Cancel)
			{
				timer.Dispose();
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

			if (n == 1)
			{
				var targetItem = targetItems.Single();
				
				props.selectedItemLabel.Text = targetItem.SourceFileInfo.Name;

				props.targetPathTextBox.Text = targetItem.TargetPath;

				props.sourceTextBox.Text = targetItem.SourcePath;
			}
			else if (n > 0)
			{
				props.selectedItemLabel.Text = string.Format(multipleItemsSelected_key, n);

				props.sourceTextBox.Text = multipleSourcePaths_key;

				var firstItem = targetItems.First();

				if (targetItems.All(i => i.TargetPath == firstItem.TargetPath))
				{
					props.targetPathTextBox.Text = firstItem.TargetPath;
				}
				else
				{
					props.targetPathTextBox.Text = multipleTargetPaths_key;
				}
			}

			var result = props.ShowDialog();

			if (result == DialogResult.OK)
			{
				foreach (var item in targetItems)
				{
					item.TargetPath = props.targetPathTextBox.Text;
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
				notifyIcon.ShowBalloonTip(2500, synchronized_key, string.Format(synchronizedStatus_key, syncCounter), ToolTipIcon.Info);
			}

			currentActionProgressBar.Visible = false;

			applicationStatusLabel.Text = idle_key;
		}

		internal void Initialize()
		{
			activeConfigurationComboBox.SelectedIndex = 0;
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
	}
}
