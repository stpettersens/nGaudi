/*
nGaudi platform agnostic build tool on .NET
Copyright (C) 2011 Sam Saint-Pettersen.

nGaudi is a .NET rewrite of the original Gaudi tool which was written for
the Java Virtual Machine (JVM).

Licensed under the MIT/X11 License.
For dependencies, please see LICENSE file.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace org.stpettersens.nGaudi
{
    class GaudiForeman
    {
        GaudiLogger l = new GaudiLogger(true);
        public GaudiForeman(string buildConf)
        {
           string buildJson = JsonConvert.SerializeObject(buildConf);
           l.Dump(String.Format("SERIALIZED\n{0}", buildJson));
        }

    }
}
