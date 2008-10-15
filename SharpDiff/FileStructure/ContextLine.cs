namespace SharpDiff.FileStructure
{
    public class ContextLine : ILine
    {
        public ContextLine(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}