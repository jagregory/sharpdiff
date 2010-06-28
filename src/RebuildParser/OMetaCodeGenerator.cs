using System;
using System.IO;
using OMetaSharp;

namespace SharpDiff.Utils.RebuildParser
{
    public class OMetaCodeGenerator
    {
        public void RebuildGitParser()
        {
            var contents = File.ReadAllText(@"..\..\..\SharpDiff\Parsers\GitDiffParser.ometacs");
            var result = Grammars.ParseGrammarThenOptimizeThenTranslate
                <OMetaParser, OMetaOptimizer, OMetaTranslator>
                (contents,
                 p => p.Grammar,
                 o => o.OptimizeGrammar,
                 t => t.Trans);

            File.WriteAllText(@"..\..\..\SharpDiff\Parsers\GitDiffParser.cs", result);
        }

        public void RebuildGitNumstatParser()
        {
            var contents = File.ReadAllText(@"..\..\..\SharpDiff\Parsers\GitNumstatParser.ometacs");
            var result = Grammars.ParseGrammarThenOptimizeThenTranslate
                <OMetaParser, OMetaOptimizer, OMetaTranslator>
                (contents,
                 p => p.Grammar,
                 o => o.OptimizeGrammar,
                 t => t.Trans);

            File.WriteAllText(@"..\..\..\SharpDiff\Parsers\GitNumstatParser.cs", result);
        }
    }
}