using System;
using System.IO;
using OMetaSharp;

namespace SharpDiff
{
    public class OMetaDiffParser
    {
        public void something()
        {
            var contents = File.ReadAllText("Diff.ometacs");
            var result = Grammars.ParseGrammarThenOptimizeThenTranslate
                <OMetaParser, OMetaOptimizer, OMetaTranslator>
            (contents,
                p => p.Grammar,
                o => o.OptimizeGrammar,
                t => t.Trans);

            File.WriteAllText(@"C:\Development\SharpDiff\SharpDiff\Generated\Diff.cs", result);
        }

        public void Somethingelse()
        {
            var test = Grammars.ParseWith<DiffParser>("diff --git", x => x.Header);

            Console.WriteLine(test.HeadFirstItem.As<Header>());
        }
    }
}