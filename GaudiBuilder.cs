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
    class GaudiBuilder
    {
        bool beVerbose;
        GaudiLogger logger;

        public GaudiBuilder(Object preamble, bool sSwitch, 
        bool beVerbose, bool logging)
        {
            this.beVerbose = beVerbose;

            // Define global logging object
            logger = new GaudiLogger(logging);
        }
        // Print an error related to action or command and exit
        void PrintError(string error)
        {
            Console.WriteLine("\tAborting: {0}", error);
            logger.Dump(error); // Also log it
            Environment.Exit(-2); // Exit application with error code
        }
        // Print executed command
        void PrintCommand(string command, string param)
        {
            if (beVerbose && command != "echo")
            {
                Console.WriteLine("\t:{0} {1}", command, param);
            }
        }
        // Execute an external process
        void ExecExternal(string param)
        {
            logger.Dump(String.Format("Executed -> {0}", param));
            Process p = new Process();
            p.StartInfo.FileName = param;
            p.Start();
        }
        // Execute a command in the action
        public void DoCommand(string command, string param)
        {
            // Do not print "echo" commands, but do print others
            PrintCommand(command, param);
            switch (command)
            {
                case "exec":
                    ExecExternal(param);
                    break;
                case "mkdir":
                    DirectoryInfo aDir = new DirectoryInfo("aDir"); // (param);
                    if (aDir.Exists)
                    {
                        PrintError("Directory already exists.");
                    }
                    aDir.Create();
                    break;
                default:
                    PrintError(String.Format("{0} is an invalid command", command));
                    break;
            }
        }
    }
}
