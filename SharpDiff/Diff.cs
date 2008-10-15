namespace SharpDiff
{
    public class Diff
    {
        private Header header;

        public Diff(Header header, Chunk chunk)
        {
            this.header = header;
            Chunk = chunk;
        }

        public Chunk Chunk { get; private set; }
    }
}