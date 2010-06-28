using SharpDiff.FileStructure;

namespace SharpDiff.FileStructure
{
    public class NoNewLineAtEOFLine : ILine
    {
        public string Value
        {
            get { return "No newline at end of file"; }
        }
    }
}