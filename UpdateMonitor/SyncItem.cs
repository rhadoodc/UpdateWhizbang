using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Sprocket.UpdateMonitor
{
	[Serializable]
	public class SyncItem
	{
		const string exception_key = "Exception...";
		const string sourcePathException_key = "An exception occured trying to get SOURCE file info for '{0}':\n{1}";
		const string targetPathException_key = "An exception occured trying to get TARGET file info for '{0}':\n{1}";

		const string syncException_key = "An exception occured trying to copy {0} to {1}:\n{2}";

		public enum SyncState
		{
			None,
			SourceException,
			TargetException,
			TargetReadOnly,
			TargetModifiedExternally,
			SourceNotFound,
			TargetNotFound,
			UpToDate,
			Outdated,
		}

		[NonSerialized]
		[XmlIgnore]
		public SyncState lastSyncState = SyncState.None;

		[NonSerialized]
		private FileInfo sourceFileInfo = null;
		
		[NonSerialized]
		private FileInfo targetFileInfo = null;

		public FileInfo SourceFileInfo
		{
			get
			{
				return sourceFileInfo;
			}
		}

		public string SourcePath
		{
			get
			{
				return sourceFileInfo.FullName;
			}

			set
			{
				try
				{
					sourceFileInfo = new FileInfo(value);
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(string.Format(sourcePathException_key, value, ex.Message), exception_key, MessageBoxButtons.OK);
					lastSyncState = SyncState.TargetException;
				}
			}
		}

		private string targetPath;

		public string TargetPath
		{
			get
			{
				return targetPath;
			}

			set
			{
				targetPath = value;
			}
		}

		public SyncItem()
		{

		}

		public SyncItem(string _sourcePath)
		{
			SourcePath = _sourcePath;
		}

		public SyncState CheckUpToDate()
		{
			lastSyncState = CheckUpToDate_Internal();

			return lastSyncState;
		}

		private SyncState CheckUpToDate_Internal()
		{
			var TargetFileInfo = new FileInfo(Path.Combine(targetPath, sourceFileInfo.Name));

			if (!SourceFileInfo.Exists)
			{
				return SyncState.SourceNotFound;
			}

			if (!TargetFileInfo.Exists)
			{
				return SyncState.TargetNotFound;
			}

			if (TargetFileInfo.IsReadOnly)
			{
				return SyncState.TargetReadOnly;
			}

			if (SourceFileInfo.LastWriteTime > TargetFileInfo.LastWriteTime)
			{
				return SyncState.Outdated;
			}
			else if ((SourceFileInfo.LastWriteTime < TargetFileInfo.LastWriteTime)
				&& (SourceFileInfo.Length != TargetFileInfo.Length))
			{
				return SyncState.TargetModifiedExternally;
			}

			return SyncState.UpToDate;
		}

		public bool Sync()
		{
			var state = CheckUpToDate();

			var doWork = (state == SyncState.Outdated) || (state == SyncState.TargetNotFound);

			try
			{
				if (doWork)
				{
					File.Copy(SourcePath, Path.Combine(TargetPath, SourceFileInfo.Name), true);
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(string.Format(syncException_key, SourcePath, TargetPath, ex.Message), exception_key, MessageBoxButtons.OK);
			}

			return doWork;
		}
	}
}
