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
    /// <summary>
    /// The GaudiBase class provides common methods to many classes.
    /// </summary>
    class GaudiBase
    {
        static bool logging = false;
        static bool beVerbose = true;
        const int ErrCode = -2;
        const string LogFile = "gaudi.log";

        /// <summary>
        /// Dump program feedback to the log file.
        /// </summary>
        /// <param name="message">Message to write to the log file.</param>
        protected void LogDump(string message)
        {
            DateTime timestamp = DateTime.Now;
            if (IsLogging())
            {
                WriteToFile(
                    LogFile, String.Format("[{0}]\r\n{1}\r\n", timestamp, message), true
                );
            }
        }

        /// <summary>
        /// Write text to a file.
        /// </summary>
        /// <param name="file">File to write to.</param>
        /// <param name="message">Message to write to file.</param>
        /// <param name="append">Append to existing file.</param>
        protected void WriteToFile(string file, string message, bool append)
        {
            StreamWriter output;
            if (append) output = File.AppendText(file);
            else output = File.CreateText(file);
            try
            {
                output.Write(message);
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

        /// <summary>
        /// Enable/disable logging.
        /// </summary>
        /// <param name="enabled">Enabled logging.</param>
        public void EnableLogging(bool enabled)
        {
            if (enabled) logging = true;
            else logging = false;
        }

        /// <summary>
        /// Enable/disable verbosity.
        /// </summary>
        /// <param name="enabled">Enabled verbosity.</param>
        public void EnableVerbosity(bool enabled)
        {
            if (enabled) beVerbose = true;
            else beVerbose = false;
        }

        // Return vebosity status
        public bool IsVerbose()
        {
            if (beVerbose) return true;
            else return false;
        }
        // Return logging status
        public bool IsLogging()
        {
            if (logging) return true;
            else return false;
        }

        /// <summary>
        /// Print an error related to action or command and exit.
        /// </summary>
        /// <param name="error">Error to print.</param>
        protected void PrintError(string error)
        {
            Console.WriteLine("\tAborting: {0}.", error);
            LogDump(error); // Also log it
            Environment.Exit(ErrCode); // Exit application with error code
        }

        /// <summary>
        /// Display an error which results in showing usage instructions.
        /// </summary>
        /// <param name="ex">Exception message to display.</param>
        public void DisplayUsageError(Exception ex)
        {
            Console.WriteLine("\nError: {0}", ex.Message);
            LogDump(ex.Message);
            GaudiApp.DisplayUsage(ErrCode);
        }

        /// <summary>
        /// Display an error which results in showing usage instructions.
        /// </summary>
        /// <param name="ex">Exception message to display.</param>
        public void DisplayUsageError(string ex)
        {
            Console.WriteLine("\nError: {0}", ex);
            LogDump(ex);
            GaudiApp.DisplayUsage(ErrCode);
        }
    }
}
