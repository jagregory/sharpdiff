using System;
using System.IO;
using OMetaSharp;

namespace SharpDiff.RebuildParser
{
    public class OMetaCodeGenerator
    {
        public void RebuildGitParser()
        {
            var contents = File.ReadAllText(@"..\..\..\SharpDiff\Parser\GitDiffParser.ometacs");
            var result = Grammars.ParseGrammarThenOptimizeThenTranslate
                <OMetaParser, OMetaOptimizer, OMetaTranslator>
                (contents,
                 p => p.Grammar,
                 o => o.OptimizeGrammar,
                 t => t.Trans);

            File.WriteAllText(@"..\..\..\SharpDiff\Parser\GitDiffParser.cs", result);
        }
    }
}