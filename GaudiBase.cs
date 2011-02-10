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

namespace Stpettersens.nGaudi
{
    class GaudiBase
    {
        //GaudiLogger logger;
        static bool logging = false;
        static bool beVerbose = true;
        const int errCode = -2;

        public GaudiBase()
        {
            //logger = new GaudiLogger();
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
            //logger.Dump(error); // Also log it
            Environment.Exit(errCode); // Exit application with error code
        }
        // Display an error which results in showing usage instructions
        public void DisplayUsageError(Exception ex)
        {
            Console.WriteLine("\nError: {0}", ex.Message);
            //logger.Dump(ex.Message);
            //GaudiApp.DisplayUsage(errCode);
        }
        // Overloaded for string parameter
        public void DisplayUsageError(string ex)
        {
            Console.WriteLine("\nError: {0}", ex);
            //logger.Dump(ex);
            //GaudiApp.DisplayUsage(errCode);
        }
    }
}
