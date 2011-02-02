/*
nGaudi platform agnostic build tool on .NET
Copyright (C) 2011 Sam Saint-Pettersen.

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
using System.IO;

namespace org.stpettersens.nGaudi
{
    class GaudiApp
    {
        // ------------------------------------------------------------------
        private static string appVersion = "0.1";
        private static int errCode = -2;
        // ------------------------------------------------------------------
        private static string buildFile = "build.json"; // Default build file
        private static bool beVerbose = true; // nGaudi is verbose by default
        private static bool logging = false;
        private static GaudiLogger logger = new GaudiLogger(logging);
        private static bool sSwitch = false;

        static void Main(string[] args)
        { 
            //
            logger.dump("Write to the log.");
            //
            bool pSwitch = false;
            bool fSwitch = false;
            string action = "build";
            Regex pluginPattn = new Regex(@"[\w:\/]+.gpod");
            Regex filePattn = new Regex(@"\w:\/]+.json");
            Regex actPattn = new Regex(@"[a-z]+");
            //Regex cmdPattn = new Regex(@"([a-z]+)\s{1}([\/A-Za-z0-9\s\.\*\+\_\-\>\!\,]+)");
            string cmd, param;

            /* Default behavior is to build project following
            build file in the current directory */
            if (args.Length == 0) loadBuild(action);

            // Handle command line arguments
            else if (args.Length > 0 && args.Length < 7)
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-i":
                            displayUsage(0);
                            break;
                        case "-v":
                            displayVersion();
                            break;
                        default:
                            break;

                    }
                }
            }
        }
        private static void runCommand(string cmd, string param) 
        {

        }
        private static void loadBuild(string action)
        {

        }
        private static void generateBuildFile()
        {
            // TODO
        }
        private static void doPluginAction()
        {

        }
        // Display an error
        public static void displayError(Exception ex)
        {
            Console.WriteLine("\nError with: {0}.", ex.Message);
            logger.dump(ex.Message);
            displayUsage(errCode);
        }
        // Overloaded for string parameter
        public static void displayError(string ex)
        {
            Console.WriteLine("\nError with: {0}.", ex);
            logger.dump(ex);
            displayUsage(errCode);
        }
        // Display version information and exit
        private static void displayVersion()
        {
            Console.WriteLine("nGaudi v.{0}", appVersion);
            System.Environment.Exit(0);
        }
        private static void displayUsage(int exitCode) 
        {
            Console.WriteLine("\nnGaudi platform agnostic build tool");
	        Console.WriteLine("Copyright (c) 2011 Sam Saint-Pettersen");
	        Console.WriteLine("\nReleased under the MIT/X11 License.");
	        Console.WriteLine("\nUsage: ngaudi [-s <port>][-l][-i|-v|-n|-m][-q]");
	        Console.WriteLine("[-p <plug-in>][-f <build file>][<action>|\"<:command>\"]");
	        Console.WriteLine("\n-s: Enable listen on socket (Default: TCP/3082).");
	        Console.WriteLine("-l: Enable logging of certain events.");
	        Console.WriteLine("-i: Display usage information and quit.");
	        Console.WriteLine("-v: Display version information and quit.");
	        Console.WriteLine("-n: Generate native Gaudi build file (build.json).");
            Console.WriteLine("-p: Invoke <plug-in> action.");
	        Console.WriteLine("-q: Mute console output, except for :echo and errors (Quiet mode).");
	        Console.WriteLine("-f: Use <build file> instead of build.json.");
            Console.ReadLine();
	        System.Environment.Exit(exitCode);
        }
    }
}
