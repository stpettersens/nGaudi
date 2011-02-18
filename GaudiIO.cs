/*
nGaudi platform agnostic build tool on .NET
Copyright (c) 2011 Sam Saint-Pettersen.

nGaudi is a .NET rewrite of the original Gaudi tool which was written for
the Java Virtual Machine (JVM).

Licensed under the MIT/X11 License.
For dependencies, please see LICENSE file.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Stpettersens.nGaudi
{

    class GaudiIO : GaudiBase
    {
        protected enum DirActions { CREATE, ERASE };
        protected enum FileActions { CREATE, ERASE };

        // File manipulation operations
        protected void ManipulateFile(string file, FileActions action)
        {
            FileInfo aFile = new FileInfo(file);
            switch (action)
            {
                case FileActions.CREATE: // Create a blank file
                    if (aFile.Exists)
                    {
                        PrintError(String.Format("File \'{0}\' already exists", file));
                    }
                    aFile.Create();
                    break;

                case FileActions.ERASE: // Erase a file
                    if (!aFile.Exists)
                    {
                        PrintError(String.Format("File \'{0}\' does not exist", file));
                    }
                    aFile.Delete();
                    break;
            }
        }
        // Directory manipulation operations
        protected void ManipulateDir(string dir, DirActions action)
        {
            DirectoryInfo aDir = new DirectoryInfo(dir);
            switch (action)
            {
                case DirActions.CREATE: // Create a directory
                    if (aDir.Exists)
                    {
                        PrintError(String.Format("Directory \'{0}\' already exists", dir));
                    }
                    aDir.Create();
                    break;
                case DirActions.ERASE: // Erase a directory
                    if (!aDir.Exists)
                    {
                        PrintError(String.Format("Directory \'{0}\' does not exist", dir));
                    }
                    aDir.Delete();
                    break;
            }
        }

        // Execute an external process
        protected void ExecExtern(string param)
        {
            LogDump(String.Format("Executed -> {0}", param));
            Process p = new Process();
            p.StartInfo.FileName = param;
            p.Start();
        }
    }
}
