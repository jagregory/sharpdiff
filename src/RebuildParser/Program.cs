using SharpDiff.Utils.RebuildParser;

namespace SharpDiff.Utils.RebuildParser
{
    class Program
    {
        static void Main()
        {
            var generator = new OMetaCodeGenerator();
            
            generator.RebuildGitParser();
            generator.RebuildGitNumstatParser();
        }
    }
}