using System.Collections.Generic;

namespace SharpDiff
{
    public class Chunk
    {
        private readonly ChunkHeader header;
        private readonly ChunkRange range;

        public Chunk(ChunkHeader header, ChunkRange range, IEnumerable<ILine> lines)
        {
            this.header = header;
            this.range = range;
            Lines = new List<ILine>(lines);
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

        public IList<ILine> Lines { get; private set; }
    }
}