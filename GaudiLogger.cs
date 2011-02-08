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
    class GaudiLogger
    {
        GaudiIO io;
        string logFile = "gaudi.log"; // Name for log file
        DateTime timestamp = DateTime.Now; // Timestamp is MM-DD-YY HH:MM:SS
        bool logging;

        public GaudiLogger(bool logging)
        {
            this.logging = true; //logging;
            io = new GaudiIO(logging);
        }
        
        // Public method to dump program feedback to the log file
        public void Dump(string message)
        {
            if (this.logging)
            {
                io.WriteToFile(
                    logFile, String.Format("[{0}]\n{1}", timestamp, message), true
                );
            }
        }
    }
}
