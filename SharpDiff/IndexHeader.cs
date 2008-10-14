namespace SharpDiff
{
    public class IndexHeader
    {
        public IndexHeader(HashRange range, string mode)
        {
            Range = range;
            Mode = mode;
        }

        public HashRange Range { get; private set; }
        public string Mode { get; private set; }
    }
}