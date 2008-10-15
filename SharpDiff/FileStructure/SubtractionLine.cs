using SharpDiff.FileStructure;

namespace SharpDiff.FileStructure
{
    public class SubtractionLine : ILine
    {
        public SubtractionLine(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}