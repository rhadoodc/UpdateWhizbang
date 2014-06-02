using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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
				}
			}
		}

		public List<string> targetPaths = new List<string>();

		public SyncItem()
		{

		}

		public SyncItem(string _sourcePath)
		{
			SourcePath = _sourcePath;
		}

		public SyncState CheckUpToDate(string path)
		{
			return CheckUpToDate_Internal(path);
		}

		private SyncState CheckUpToDate_Internal(string targetPath)
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
			bool doWork = false;

			foreach (var targetPath in targetPaths)
			{
				var state = CheckUpToDate(targetPath);

				doWork |= (state == SyncState.Outdated) || (state == SyncState.TargetNotFound);

				try
				{
					if (doWork)
					{
						File.Copy(SourcePath, Path.Combine(targetPath, SourceFileInfo.Name), true);
					}
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(string.Format(syncException_key, SourcePath, targetPath, ex.Message), exception_key, MessageBoxButtons.OK);
				}
			}

			return doWork;
		}
	}
}
