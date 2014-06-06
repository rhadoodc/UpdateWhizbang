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
		private FileSystemInfo sourceFileInfo = null;

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
						sourceFileInfo = new DirectoryInfo(value);
					else
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
			FileSystemInfo TargetFileInfo = null;

			var path = Path.Combine(targetPath, sourceFileInfo.Name);
			
			var attributes = File.GetAttributes(SourcePath);

			if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
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
				var state = CheckUpToDate(targetPath);

				doWork |= (state == SyncState.Outdated) || (state == SyncState.TargetNotFound);

				try
				{
					if (doWork)
					{
						var attributes = File.GetAttributes(SourcePath);
						if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
							DirectoryCopy(SourcePath, Path.Combine(targetPath, SourceFileInfo.Name), true);
						else
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
