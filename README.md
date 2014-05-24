Update Whizbang
===============

A small application that monitors changes in a set of files and mirrors them into target locations.

Contents:
1. Description

2. Change list

3. Building from source

4. Usage

5. License

6. Credits

1. Description:

	The application was written with the goal of synchronizing Visual Studio-compiled files into Unity projects, instead of using a Post-Build step (easier to do with a GUI). Despite that, the Whizbang is completely agnostic of file types, and can be used for any workflow that requires files from a source location be moved to a target location frequently. Locations are set per-file, and there's also support for multiple configurations that synchronize different sets of files (Debug/Release/etc.).

2. Change list:

	v0.1 - First Version
	
3. Building from source:

	If you have obtained the application as source files, just open the solution file (UpdateManager.sln) using Visual Studio (2012 or later tested to work) and trigger a build for the configuration you would like to use (Release, most probably); you will then find the binaries inside the 'bin/' folder.

4. Usage:

	Upon first run of the application it will prompt you to create a configuration - configurations are separate sets of items to synchronize. After you have created a configuration, you can begin adding files to it by dragging them into the drop view. As soon as you drop a file (or several) in, the properties view will show, asking you to set a target folder to sync the file(s) to. Once you've set a target path click apply. You can rinse and repeat the process to add more files.
	
	Every few seconds the Whizbang will check for changes to the 'source' files you have added to the currently active configuration (the one selected in the upper left combo box), and if any are detected, it will copy the files to the target directory.
	
	Should you need to remove files from the active configuration, you can do so by selecting the files, right clicking one of them and selecting 'Delete'.
	
	If you want to force synchronization RIGHT NOW (you must really be in a hurry, the Whizbang really does this every few seconds), you can do it for just the selected files from the context menu, or by clicking the button in the upper right corner. You can also force sync the entire active configuration by right clicking the Whizbang's icon in the notification area and selecting the 'Sync All' option.
	
	The Whizbang cowers to the notification area if you try to close or minimize its window. In order to banish it for good (quit) you can right-click the icon in the notification area and select 'Quit'; of course if you're feeling particularly brutal you can also dismiss it by ending the process from the Task Manager, or using 'taskkill' from the command prompt. The latter two should be used sparingly though (really, what has the Whizbang ever done to you?) as you may end up interrupting file copy operations, breaking the target file(s) (in which case you get to keep all the resulting pieces).

5. License:

	The license ruling over this bulk of code is "The Unlicense", the text of which can be found in the adjacent LICENSE file.
	
6. Credits:

	The icons giving the Whizbang its rugged good looks were obtained from http://www.famfamfam.com/lab/icons/silk/
