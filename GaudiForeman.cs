/*
nGaudi platform agnostic build tool on .NET
Copyright (c) 2011 Sam Saint-Pettersen.

nGaudi is a .NET rewrite of the original Gaudi tool which was written for
the Java Virtual Machine (JVM).

Licensed under the MIT/X11 License.
For dependencies, please see LICENSE file.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Jayrock.Json;

namespace Stpettersens.nGaudi
{
    class GaudiForeman : GaudiBase
    {
        string buildConf; //, buildJson;
        //ArrayList[] actions;

        public GaudiForeman(string buildConf)
        {
            this.buildConf = buildConf;

            // Parse build config into JSON Hashtable on initialization
            //buildJson = ParseBuildJson();
            //ParseEachAction();   
        }
        public string ParseBuildJson()
        {
            JsonTextReader reader = new JsonTextReader(new StreamReader(buildConf));
            return "string";
        }
    }
}
