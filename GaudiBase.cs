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
        GaudiLogger logger;
        protected bool logging;
        const int errCode = -2;

        public GaudiBase()
        {
            logger = new GaudiLogger(logging);
        }
        
        // Print an error related to action or command and exit
        protected void PrintError(string error)
        {
            Console.WriteLine("\tAborting: {0}.", error);
            logger.Dump(error); // Also log it
            Environment.Exit(GaudiApp.errCode); // Exit application with error code
        }
        // Display an error which results in showing usage instructions
        protected void DisplyUsageError(Exception ex)
        {
            Console.WriteLine("\nError: {0}", ex.Message);
            logger.Dump(ex.Message);
            GaudiApp.DisplayUsage(errCode);
        }
        // Overloaded for string parameter
        protected void DisplayUsageError(string ex)
        {
            Console.WriteLine("\nError: {0}", ex);
            logger.Dump(ex);
            GaudiApp.DisplayUsage(errCode);
        }
    }
}
