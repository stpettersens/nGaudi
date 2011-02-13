/*
nGaudi platform agnostic build tool on .NET
Copyright (c) 2011 Sam Saint-Pettersen.

nGaudi is a .NET rewrite of the original Gaudi tool which was written for
the Java Virtual Machine (JVM).

Licensed under the MIT/X11 License.
For dependencies, please see LICENSE file.
*/
using System;
using System.Text;

namespace Stpettersens.nGaudi
{
    static class GaudiPluginSupport
    {
        public enum PluginLang { IronPython, Boo };

        // Code to enable or disable plug-in support.
        public const bool Enabled = true;

        // Plugin language
        public static PluginLang GetPluginLang() 
        {
            // Use IronPython for .NET 4 Framework
            if (Environment.Version.Major.ToString() == "4")
            {
               return PluginLang.IronPython;
            }
            // Use Boo for .NET 3.5 Framework or Mono
            else return PluginLang.Boo; 
        }
    }
}
