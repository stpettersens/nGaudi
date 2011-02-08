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
using System.Text.RegularExpressions;


namespace Stpettersens.nGaudi
{
    class GaudiBuilder
    {
        bool beVerbose;
        static GaudiLogger logger;
        GaudiIO io;

        public GaudiBuilder(Object preamble, bool sSwitch, 
        bool beVerbose, bool logging)
        {
            this.beVerbose = beVerbose;

            // Define global logging object
            logger = new GaudiLogger(logging);

            // Define global IO object
            io = new GaudiIO(logging);
        }
        // Print an error related to action or command and exit
        public static void PrintError(string error)
        {
            Console.WriteLine("\tAborting: {0}.", error);
            logger.Dump(error); // Also log it
            Environment.Exit(GaudiApp.errCode); // Exit application with error code
        }
        // Print executed command
        void PrintCommand(string command, string param)
        {
            if (beVerbose && command != "echo")
            {
                Console.WriteLine("\t:{0} {1}", command, param);
            }
        }

 
        // Execute a command in the action
        public void DoCommand(string command, string param)
        {
            // Do not print "echo" commands, but do print others
            PrintCommand(command, param);
            switch (command)
            {
                case "exec":
                    io.ExecExtern(param);
                    break;
                case "mkdir":
                    io.ManipulateDir(param, 0);
                    break;
                case "rmdir":
                    io.ManipulateDir(param, 1);
                    break;
                case "list":
                    // TODO
                    break;
                case "echo":
                    Console.WriteLine("\t# {0}", param);
                    break;
                case "erase":
                    io.ManipulateFile(param, 1);
                    break;
                case "copy":
                    // TODO
                    break;
                case "rcopy":
                    // TODO
                    break;
                case "move":
                    // TODO
                    break;
                // Append message to a file
                // Usage in buildfile: { "append": file>>message" }
                // Equivalent to *nix's echo "message" >> file
                case "append":
                    string[] fileMsg = Regex.Split(param, ">>");
                    io.WriteToFile(fileMsg[0], fileMsg[1], true);
                    break;
                default:
                    PrintError(String.Format("{0} is an invalid command", command));
                    break;
            }
        }
    }
}
