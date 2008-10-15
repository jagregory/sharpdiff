namespace SharpDiff
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