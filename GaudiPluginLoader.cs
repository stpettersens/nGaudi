﻿/*
nGaudi platform agnostic build tool on .NET
Copyright (c) 2011 Sam Saint-Pettersen.

nGaudi is a .NET rewrite of the original Gaudi tool which was written for
the Java Virtual Machine (JVM).

Licensed under the MIT/X11 License.
For dependencies, please see LICENSE file.
*/
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
    /// <summary>
    /// The GaudiPluginLoader class handles the loading of plug-ins for Gaudi.
    /// </summary>
    class GaudiPluginLoader : GaudiBase
    {
        /// <summary>
        /// Constructor for the GaudiPluginLoader class.
        /// </summary>
        /// <param name="plugin">Plugin to load.</param>
        public GaudiPluginLoader(string plugin)
        {
            // First of all, extract plugin code from the plugin archive (Zip file).
            GaudiPacker unpacker = new GaudiPacker(plugin);
            string codeFile = unpacker.ExtractZipFile();
        }
    }
}
