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
        GaudiLogger logger;
        public enum DirActions { CREATE, ERASE };
        public enum FileActions { CREATE, ERASE };

        public GaudiIO()
        {
            logger = new GaudiLogger(logging);
        }

        // File writing operations
        public void WriteToFile(string file, string message, bool append)
        {
            StreamWriter output;
            if (append) output = File.AppendText(file);
            else output = File.CreateText(file);
            try
            {
                output.WriteLine(message);
            }
            catch (IOException ioe)
            {
                PrintError(ioe.Message);
            }
            finally
            {
                output.Close();
            }
        }
        // File manipulation operations
        public void ManipulateFile(string file, FileActions action)
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
        public void ManipulateDir(string dir, DirActions action)
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
        public void ExecExtern(string param)
        {
            logger.Dump(String.Format("Executed -> {0}", param));
            Process p = new Process();
            p.StartInfo.FileName = param;
            p.Start();
        }
    }
}
