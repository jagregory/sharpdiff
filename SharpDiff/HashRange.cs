namespace SharpDiff
{
    public class HashRange
    {
        public HashRange(string start, string end)
        {
            Start = start;
            End = end;
        }

        public string Start { get; private set; }
        public string End { get; private set; }
    }
}