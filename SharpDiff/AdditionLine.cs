namespace SharpDiff
{
    public class AdditionLine : ILine
    {
        public AdditionLine(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}