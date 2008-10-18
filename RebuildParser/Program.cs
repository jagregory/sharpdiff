using SharpDiff.Utils.RebuildParser;

namespace SharpDiff.Utils.RebuildParser
{
    class Program
    {
        static void Main()
        {
            new OMetaCodeGenerator().RebuildGitParser();
        }
    }
}