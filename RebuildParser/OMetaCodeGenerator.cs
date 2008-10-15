using System;
using System.IO;
using OMetaSharp;

namespace SharpDiff.RebuildParser
{
    public class OMetaCodeGenerator
    {
        public void Rebuild()
        {
            var contents = File.ReadAllText(@"..\..\..\SharpDiff\Parser\DiffParser.ometacs");
            var result = Grammars.ParseGrammarThenOptimizeThenTranslate
                <OMetaParser, OMetaOptimizer, OMetaTranslator>
                (contents,
                 p => p.Grammar,
                 o => o.OptimizeGrammar,
                 t => t.Trans);

            File.WriteAllText(@"..\..\..\SharpDiff\Parser\DiffParser.cs", result);
        }
    }
}