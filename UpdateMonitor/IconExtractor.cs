using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sprocket.UpdateMonitor
{
	public static class IconExtractor
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		struct SHFILEINFO 
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}

		[DllImport("shell32.dll")]
		private static extern IntPtr SHGetFileInfo(string pszPath,
													uint dwFileAttributes,
													ref SHFILEINFO psfi,
													uint cbSizeFileInfo,
													uint uFlags);

		[DllImport("Shell32.dll")]
		public extern static int ExtractIconEx(
			string libName,
			int iconIndex,
			IntPtr[] largeIcon,
			IntPtr[] smallIcon,
			uint nIcons
		);

		public enum ShellIconSize
		{
			LargeIcon = 0x100,    // 32x32 pixels
			SmallIcon = 0x101,    // 16x16 pixels
			ExtraLarge = 0x102,
			SysSmall = 0x103,
			Jumbo = 0x104
		}

		public static Icon GetIconForFile(string filename, ShellIconSize size)
		{
			SHFILEINFO shinfo = new SHFILEINFO();
			SHGetFileInfo(filename, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)size);
			return Icon.FromHandle(shinfo.hIcon);
		}

		public static Icon GetIconForExtension(string extension, ShellIconSize size)
		{
			RegistryKey keyForExt = Registry.ClassesRoot.OpenSubKey(extension);
			if (keyForExt == null) return null;

			string className = Convert.ToString(keyForExt.GetValue(null));
			RegistryKey keyForClass = Registry.ClassesRoot.OpenSubKey(className);
			if (keyForClass == null) return null;

			RegistryKey keyForIcon = keyForClass.OpenSubKey("DefaultIcon");
			if (keyForIcon == null)
			{
				RegistryKey keyForCLSID = keyForClass.OpenSubKey("CLSID");
				if (keyForCLSID == null) return null;

				string clsid = "CLSID\\"
					+ Convert.ToString(keyForCLSID.GetValue(null))
					+ "\\DefaultIcon";
				keyForIcon = Registry.ClassesRoot.OpenSubKey(clsid);
				if (keyForIcon == null) return null;
			}

			string[] defaultIcon = Convert.ToString(keyForIcon.GetValue(null)).Split(',');
			int index = (defaultIcon.Length > 1) ? Int32.Parse(defaultIcon[1]) : 0;

			IntPtr[] handles = new IntPtr[1];
			if (ExtractIconEx(defaultIcon[0], index,
				(size == ShellIconSize.LargeIcon) ? handles : null,
				(size == ShellIconSize.SmallIcon) ? handles : null, 1) > 0)
				return Icon.FromHandle(handles[0]);
			else
				return null;
		}
	}
}
