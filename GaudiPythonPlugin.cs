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
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Stpettersens.nGaudi
{

    class GaudiPythonPlugin : GaudiIO
    {
        ScriptEngine iPy;

        public GaudiPythonPlugin(string plugin)
        {
            try
            {
                iPy = Python.CreateEngine();
                iPy.CreateScope();
                iPy.ExecuteFile(String.Format("{0}.py", plugin));
            }
            catch (Exception ex)
            {
                PrintError(String.Format("Script error - {0}.", ex.Message));
                LogDump(ex.Message);
            }
        }
    }
}
