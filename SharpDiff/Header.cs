namespace SharpDiff
{
    public class Header
    {
        public Header(FormatType format)
        {
            Format = format;
        }

        public FormatType Format { get; private set; }

        public override string ToString()
        {
            return "Header: " + Format.Name;
        }
    }
}