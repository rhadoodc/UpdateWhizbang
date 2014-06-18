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

		private FileSystemWatcher fileSystemWatcher = null;

		private bool isDirectory = false;

		public List<string> targetPaths = new List<string>();

		[NonSerialized]
		private FileSystemInfo sourceFileInfo = null;

		public delegate void SyncItemChangeDelegate(string itemPath);
		public delegate void SyncItemRenameDelegate(string itemOldPath, string itemNewPath);

		public event SyncItemChangeDelegate SyncItemSourceChanged;
		public event SyncItemRenameDelegate SyncItemSourceRenamed;
		public event SyncItemChangeDelegate SyncItemSourceDeleted;
		public event SyncItemChangeDelegate SyncItemSourceError;

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

		public FileSystemInfo SourceFileInfo
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
					var attributes = File.GetAttributes(value);

					if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
					{
						isDirectory = true;

						sourceFileInfo = new DirectoryInfo(value);

						if (fileSystemWatcher != null)
						{
							fileSystemWatcher.Changed -= fileSystemWatcher_Changed;
							fileSystemWatcher.Deleted -= fileSystemWatcher_Deleted;
							fileSystemWatcher.Renamed -= fileSystemWatcher_Renamed;
							fileSystemWatcher.Error -= fileSystemWatcher_Error;

							fileSystemWatcher.Dispose();
						}

						fileSystemWatcher = new FileSystemWatcher(value);

						fileSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName;
					}
					else
					{
						sourceFileInfo = new FileInfo(value);

						if (fileSystemWatcher != null)
						{
							fileSystemWatcher.Changed -= fileSystemWatcher_Changed;
							fileSystemWatcher.Deleted -= fileSystemWatcher_Deleted;
							fileSystemWatcher.Renamed -= fileSystemWatcher_Renamed;
							fileSystemWatcher.Error -= fileSystemWatcher_Error;

							fileSystemWatcher.Dispose();
						}

						fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(sourceFileInfo.FullName), sourceFileInfo.Name);

						fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;

						fileSystemWatcher.EnableRaisingEvents = true;
					}

					fileSystemWatcher.NotifyFilter |= NotifyFilters.Attributes | NotifyFilters.LastWrite;

					fileSystemWatcher.Changed += fileSystemWatcher_Changed;
					fileSystemWatcher.Deleted += fileSystemWatcher_Deleted;
					fileSystemWatcher.Renamed += fileSystemWatcher_Renamed;
					fileSystemWatcher.Error += fileSystemWatcher_Error;
				}
				catch (System.Exception ex)
				{
					Program.Log(string.Format(sourcePathException_key, value, ex.Message), exception_key);

					MessageBox.Show(string.Format(sourcePathException_key, value, ex.Message), exception_key, MessageBoxButtons.OK);
				}
			}
		}

		void fileSystemWatcher_Error(object sender, ErrorEventArgs e)
		{
			var eh = SyncItemSourceError;

			if (eh != null)
			{
				eh(SourceFileInfo.FullName);
			}
		}

		void fileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
		{
			var eh = SyncItemSourceDeleted;

			if (eh != null)
			{
				eh(SourceFileInfo.FullName);
			}
		}

		void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			bool didWork = Sync();

			if (didWork)
			{
				var eh = SyncItemSourceChanged;

				if (eh != null)
				{
					eh(SourceFileInfo.FullName);
				}
			}
		}

		void fileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
		{
			if (SourcePath == e.OldFullPath)
			{
				SourcePath = e.FullPath;

				var eh = SyncItemSourceRenamed;

				if (eh != null)
				{
					eh(e.OldFullPath, e.FullPath);
				}
			}
		}

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
			FileSystemInfo TargetFileInfo = null;

			SourceFileInfo.Refresh();

			var path = Path.Combine(targetPath, SourceFileInfo.Name);

			if (isDirectory)
				TargetFileInfo = new DirectoryInfo(path);
			else
				TargetFileInfo = new FileInfo(path);

			if (!SourceFileInfo.Exists)
			{
				return SyncState.SourceNotFound;
			}

			if (!TargetFileInfo.Exists)
			{
				return SyncState.TargetNotFound;
			}

			if ((TargetFileInfo is FileInfo) && (((FileInfo)TargetFileInfo).IsReadOnly))
			{
				return SyncState.TargetReadOnly;
			}

			if (SourceFileInfo.LastWriteTime > TargetFileInfo.LastWriteTime)
			{
				return SyncState.Outdated;
			}
			else if (SourceFileInfo.LastWriteTime < TargetFileInfo.LastWriteTime)
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
				SyncState state = CheckUpToDate(targetPath);

				doWork = doWork || ((state == SyncState.Outdated) || (state == SyncState.TargetNotFound));

				try
				{
					if (doWork)
					{
						if (isDirectory)
							DirectoryCopy(SourcePath, Path.Combine(targetPath, SourceFileInfo.Name), true);
						else
							File.Copy(SourcePath, Path.Combine(targetPath, SourceFileInfo.Name), true);
					}
				}
				catch (System.Exception ex)
				{
					Program.Log(string.Format(syncException_key, SourcePath, targetPath, ex.Message), exception_key);

					MessageBox.Show(string.Format(syncException_key, SourcePath, targetPath, ex.Message), exception_key, MessageBoxButtons.OK);
				}
			}

			return doWork;
		}

		private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = SourceFileInfo as DirectoryInfo;
			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the destination directory doesn't exist, create it. 
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			// If copying subdirectories, copy them and their contents to new location. 
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}
	}
}
