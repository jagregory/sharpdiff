namespace SharpDiff
{
    public class Chunk
    {
        private readonly ChunkHeader header;

        public Chunk(ChunkHeader header)
        {
            this.header = header;
        }

        public FileDef OriginalFile
        {
            get { return header.OriginalFile; }
        }

        public FileDef NewFile
        {
            get { return header.NewFile; }
        }
    }
}