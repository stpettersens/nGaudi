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
    class GaudiBuilder
    {
        public GaudiBuilder(Object preamble, bool sSwitch, bool beVerbose, bool logging)
        {
            // Define global logging object
            GaudiLogger logger = new GaudiLogger(logging);
        }
        // Execute a command in the action
        public void DoCommand(string command, string param)
        {

        }
    }
}
