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
using Newtonsoft.Json.Linq;

namespace org.stpettersens.nGaudi
{
    class GaudiForeman
    {
        string buildConf;
        public GaudiForeman(string buildConf)
        {
            this.buildConf = buildConf;
            JObject buildJson = this.ParseBuildJson();
        }
        JObject ParseBuildJson()
        {
            JObject json = null;
            try
            {
                json = JsonConvert.DeserializeObject<JObject>(this.buildConf);
            }
            catch (JsonReaderException jex)
            {
                GaudiApp.DisplayError(
                    String.Format("Badly formatted JSON\n {0}", jex.Message)
                );
            }
            return json;
        }
    }
}
