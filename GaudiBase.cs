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
using System.IO;

namespace Stpettersens.nGaudi
{
    class GaudiBase
    {
        static bool logging = false;
        static bool beVerbose = true;
        const int errCode = -2;
        protected string logFile = "gaudi.log";

        // Protected method to dump program feedback to the log file
        protected void LogDump(string message)
        {
            DateTime timestamp = DateTime.Now;
            if (isLogging())
            {
                WriteToFile(
                    logFile, String.Format("[{0}]\n{1}", timestamp, message), true
                );
            }
        }

        // File writing operations
        protected void WriteToFile(string file, string message, bool append)
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

        // Enable/disable logging
        public void enableLogging(bool enabled)
        {
            if (enabled) logging = true;
            else logging = false;
        }

        // Enable verbosity
        public void enableVerbosity(bool enabled)
        {
            if (enabled) beVerbose = true;
            else beVerbose = false;
        }

        // Return vebosity
        public bool isVerbose()
        {
            if (beVerbose) return true;
            else return false;
        }
        // Return logging
        public bool isLogging()
        {
            if (logging) return true;
            else return false;
        }

        // Print an error related to action or com,mand and exit
        protected void PrintError(string error)
        {
            Console.WriteLine("\tAborting: {0}.", error);
            LogDump(error); // Also log it
            Environment.Exit(errCode); // Exit application with error code
        }

        // Display an error which results in showing usage instructions
        public void DisplayUsageError(Exception ex)
        {
            Console.WriteLine("\nError: {0}", ex.Message);
            LogDump(ex.Message);
            GaudiApp.DisplayUsage(errCode);
        }

        // Overloaded for string parameter
        public void DisplayUsageError(string ex)
        {
            Console.WriteLine("\nError: {0}", ex);
            LogDump(ex);
            GaudiApp.DisplayUsage(errCode);
        }
    }
}
