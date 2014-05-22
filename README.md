Update Whizbang
===============

A small application that monitors changes in a set of files and mirrors them into target locations.

The application was written with the goal of synchronizing Visual Studio-compiled files into Unity projects, instead of using a Post-Build step (easier to do with a GUI). Despite that, the Whizbang is completely agnostic of file types, and can be used for any workflow that requires files from a source location be moved to a target location frequently. Locations are set per-file, and there's also support for multiple configurations that synchronize different sets of files (Debug/Release/etc.).

The license ruling over this bulk of code is "The Unlicense", the text of which can be found in the adjacent LICENSE file.

The icons giving the Whizbang its rugged good looks were obtained from http://www.famfamfam.com/lab/icons/silk/
