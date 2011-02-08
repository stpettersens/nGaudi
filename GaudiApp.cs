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
    static class GaudiApp
    {
        // -----------------------------------------------------------
        const string cliName = "nGaudi";
        const string appVersion = "0.1";
        public const int errCode = -2;
        // -----------------------------------------------------------
        static string buildFile = "build.json"; // Default build file
        static bool beVerbose = true; // nGaudi is verbose by default
        static bool logging = false;
        static GaudiLogger logger = new GaudiLogger(logging);
        static GaudiMessenger messenger = new GaudiMessenger(logging);
        static bool sSwitch = false;

        static void Main(string[] args)
        { 
            bool pSwitch = false;
            bool fSwitch = false;
            string action = "build";
            //Regex pluginPattn = new Regex(@"[\w:\/]+.gpod");
            //Regex filePattn = new Regex(@"\w:\/]+.json");
            //Regex actPattn = new Regex(@"[a-z]+");
            //Regex cmdPattn = new Regex(@"(:[a-z]+)\s{1}([\/A-Za-z0-9\s\.\*\+\_\-\>\!\,]+)");

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
                            logging = true;
                            break;
                        case "-s":
                            sSwitch = true;
                            break;
                        case "-n":
                            GenerateBuildFile();
                            break;
                        case "-p":
                            DoPluginAction(args[i]);
                            break;
                        case "-q":
                            beVerbose = false;
                            break;
                        case "-f":
                            buildFile = args[i];
                            break;
                    }
                    if (arg.StartsWith(":"))
                    {
                        string[] cmdParam = Regex.Split(arg, " ");
                        RunCommand(cmdParam[0].Replace(":", ""), cmdParam[1]);
                    }
                }
                if (sSwitch) messenger.Start();
                //if (cmd != null) runCommand(cmd, param);
                LoadBuild(action);
            }
            else DisplayError("Arguments (requires  0-6 arguments)");
        }
        // Just perform a stdin command; really just for testing implemented commands.
        // E.g. argument ":move a->b"
        static void RunCommand(string cmd, string param) 
        {
            // Create a new builder to run a command
            GaudiBuilder builder = new GaudiBuilder(null, sSwitch, beVerbose, logging);
            builder.DoCommand(cmd, param);
            System.Environment.Exit(0);
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
                DisplayError(ioe);
            }
            catch (Exception ex)
            {
                // Catch another exception
                DisplayError(ex);
            }
            // Delegate to the foreman and builder
            GaudiForeman foreman = new GaudiForeman(buildConf);
            GaudiBuilder builder = new GaudiBuilder(null, sSwitch, beVerbose, logging);
        }
        // Generate a Gaudi build file (build.json)
        static void GenerateBuildFile()
        {
            // TODO
        }
        static void DoPluginAction(string plugin)
        {

        }
        // Display an error
        public static void DisplayError(Exception ex)
        {
            Console.WriteLine("\nError: {0}", ex.Message);
            logger.Dump(ex.Message);
            DisplayUsage(errCode);
        }
        // Overloaded for string parameter
        public static void DisplayError(string ex)
        {
            Console.WriteLine("\nError: {0}", ex);
            logger.Dump(ex);
            DisplayUsage(errCode);
        }
        // Display version information and exit
        static void DisplayVersion()
        {
            Console.WriteLine("nGaudi v.{0} [CLR {1} ({2})]", appVersion, 
            Environment.Version, Environment.OSVersion);
            Environment.Exit(0);
        }
        // Display usage information and exit
        static void DisplayUsage(int exitCode) 
        {
            Console.WriteLine("\nnGaudi platform agnostic build tool");
            Console.WriteLine("Copyright (c) 2011 Sam Saint-Pettersen");
            Console.WriteLine("\nReleased under the MIT/X11 License.");
            Console.WriteLine("\nUsage: {0} [-s <port>][-l][-i|-v|-n|-m][-q]", cliName);
            Console.WriteLine("[-p <plug-in>][-f <build file>][<action>|\"<:command>\"]");
            Console.WriteLine("\n-s: Enable listen on socket (Default: TCP/3082).");
            Console.WriteLine("-l: Enable logging of certain events.");
            Console.WriteLine("-i: Display usage information and quit.");
            Console.WriteLine("-v: Display version information and quit.");
            Console.WriteLine("-n: Generate native Gaudi build file (build.json).");
            Console.WriteLine("-p: Invoke <plug-in> action.");
            Console.WriteLine("-q: Mute console output, except for :echo and errors (Quiet mode).");
            Console.WriteLine("-f: Use <build file> instead of build.json.");
            Environment.Exit(exitCode);
        }
    }
}
