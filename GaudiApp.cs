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
using System.IO;

namespace Stpettersens.nGaudi
{
    class GaudiApp
    {
        // ------------------------------------------------------------
        const string CliName = "nGaudi";
        const string AppVersion = "0.1";
        // ------------------------------------------------------------
        static string buildFile = "build.json"; // Default build file
        static GaudiBase b = new GaudiBase();
        static GaudiMessenger messenger;

        static void Main(string[] args)
        { 
            string action = "build";

            /* Default behavior is to build project following
            build file in the current directory */
            if (args.Length == 0) LoadBuild(action);

            // Handle command line arguments
            else if (args.Length > 0 && args.Length < 7)
            {
                int i = 0;
                foreach (string arg in args)
                {
                    i++;
                    switch (arg)
                    {
                        case "-i":
                            DisplayUsage(0);
                            break;
                        case "-v":
                            DisplayVersion();
                            break;
                        case "-l":
                            b.enableLogging(true);
                            break;
                        case "-s":
                            messenger = new GaudiMessenger(3082);
                            messenger.Start();
                            break;
                        case "-b":
                            GenerateBuildFile();
                            break;
                        case "-p":
                            DoPluginAction(args[i]);
                            break;
                        case "-q":
                            b.enableVerbosity(false);
                            break;
                        case "-f":
                            buildFile = args[i];
                            break;
                        default:
                            b.DisplayUsageError(String.Format("Invalid argument '{0}'.", arg));
                            break;
                    }
                    if (arg.StartsWith(":"))
                    {
                        string[] cmdParam = Regex.Split(arg, " ");
                        RunCommand(cmdParam[0].Replace(":", ""), cmdParam[1]);
                    }
                }
                LoadBuild(action);
            }
            else b.DisplayUsageError("Requires  0-6 arguments");
        }
        // Just perform a stdin command; really just for testing implemented commands.
        // E.g. argument ":move a->b"
        static void RunCommand(string cmd, string param) 
        {
            // Create a new builder to run a command
            GaudiBuilder builder = new GaudiBuilder();
            builder.DoCommand(cmd, param);
            Environment.Exit(0);
        }
        // Load and delegate parse and execution of build file
        static void LoadBuild(string action)
        {
            string buildConf = null;
            try
            {
                TextReader input = new StreamReader(buildFile);
                buildConf = input.ReadToEnd();
                // Shrink string, by replacing tabs with spaces;
                // Gaudi build files should be written using tabs
                buildConf = buildConf.Replace("\t", "");
            }
            catch (IOException ioe)
            {
                // Catch I/O exception
                b.DisplayUsageError(ioe);
            }
            catch (Exception ex)
            {
                // Catch another exception
                b.DisplayUsageError(ex);
            }
            // Delegate to the foreman and builder
            GaudiForeman foreman = new GaudiForeman(buildConf);
            GaudiBuilder builder = new GaudiBuilder(null);
        }
        // Generate a Gaudi build file (build.json)
        static void GenerateBuildFile()
        {
            // TODO
        }
        static void DoPluginAction(string plugin)
        {
            new GaudiPluginLoader(plugin);
            Environment.Exit(0);
        }
        // Display version information and exit
        static void DisplayVersion()
        {
            Console.WriteLine("nGaudi v.{0} [CLR {1} ({2})]", AppVersion, 
            Environment.Version, Environment.OSVersion);
            Environment.Exit(0);
        }
        // Display usage information and exit
        public static void DisplayUsage(int exitCode) 
        {
            Console.WriteLine("\nnGaudi platform agnostic build tool");
            Console.WriteLine("Copyright (c) 2011 Sam Saint-Pettersen");
            Console.WriteLine("\nReleased under the MIT/X11 License.");
            Console.WriteLine("\nUsage: {0} [-l][-s <port>][-i|-v|-n|-m][-q]", CliName);
            Console.WriteLine("[-p <plug-in>][-f <build file>][<action>|\"<:command>\"]");
            Console.WriteLine("-l: Enable logging of certain events.");
            Console.WriteLine("\n-s: Enable listen on socket (Default: TCP/3082).");
            Console.WriteLine("-i: Display usage information and quit.");
            Console.WriteLine("-v: Display version information and quit.");
            Console.WriteLine("-b: Generate Gaudi build file (build.json).");
            Console.WriteLine("-p: Invoke <plug-in> action.");
            Console.WriteLine("-q: Mute console output, except for :echo and errors (Quiet mode).");
            Console.WriteLine("-f: Use <build file> instead of build.json.");
            Environment.Exit(exitCode);
        }
    }
}
