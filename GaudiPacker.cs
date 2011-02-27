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
using System.IO;
using Ionic.Zip;

namespace Stpettersens.nGaudi
{
    class GaudiPacker : GaudiBase
    {
        string archive;

        public GaudiPacker(string arch)
        {
            this.archive = arch;
        }
        public string ExtractZipFile()
        {
            try
            {
                ZipFile zip = ZipFile.Read(archive);
                string fileName = null;
                foreach (ZipEntry entry in zip)
                {
                    entry.Extract();
                    LogDump(String.Format("Extracted file <{0}>/{1}", archive, entry.FileName));
                    fileName = entry.FileName;
                }
                return fileName;
            }
            catch (ZipException ze)
            {
                PrintError(ze.Message);
            }
            catch (FileNotFoundException fnf)
            {
                PrintError(fnf.Message);
            }
            return null;
        }
    }
}
