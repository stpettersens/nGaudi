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
using Procurios.Public;

namespace Stpettersens.nGaudi
{
    class GaudiForeman
    {
        string buildConf;
        Hashtable buildJson;
        ArrayList[] actions;

        public GaudiForeman(string buildConf)
        {
            this.buildConf = buildConf;

            // Parse build config into JSON Hashtable on initialization
            buildJson = this.ParseBuildJson();
            this.ParseEachAction();   
        }
        Hashtable ParseBuildJson()
        {
            try
            {
                return (Hashtable)JSON.JsonDecode(buildConf);
            }
            catch (Exception)
            {
                // Need to modify JSON.cs to throw exceptions
                GaudiApp.DisplayError("Badly formatted JSON instructions");
            }
            return null;
        }
        void ParseEachAction()
        {
            foreach (DictionaryEntry x in buildJson)
            {
                Console.WriteLine(x.Key);
            }
            Console.ReadLine();
        }
    }
}
