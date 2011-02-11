using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;

namespace Stpettersens.nGaudi
{
    class GaudiBooPlugin : GaudiIO
    {
        BooCompiler booc;
        CompilerContext context;

        public GaudiBooPlugin(string plugin)
        {
            try
            {
                booc = new BooCompiler();
                booc.Parameters.Input.Add(
                    new FileInput(String.Format("{0}.boo", plugin))
                );
                booc.Parameters.Pipeline = new CompileToBoo();
                booc.Parameters.Ducky = true;
                context = booc.Run();

                // TODO
            }
            catch (CompilerError bce)
            {
                PrintError("Compiler error - " + bce.Message);
                LogDump(bce.Message);
            }
        }
    }
}
