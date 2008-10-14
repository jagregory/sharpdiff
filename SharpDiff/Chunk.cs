namespace SharpDiff
{
    public class Chunk
    {
        private readonly ChunkHeader header;
        private readonly ChunkRange range;

        public Chunk(ChunkHeader header, ChunkRange range)
        {
            this.header = header;
            this.range = range;
        }

        public FileDef OriginalFile
        {
            get { return header.OriginalFile; }
        }

        public FileDef NewFile
        {
            get { return header.NewFile; }
        }

        public ChangeRange OriginalRange
        {
            get { return range.OriginalRange; }
        }

        public ChangeRange NewRange
        {
            get { return range.NewRange; }
        }
    }
}