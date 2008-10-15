using System.Collections.Generic;

namespace SharpDiff
{
    public class Chunk
    {
        private readonly ChunkRange range;

        public Chunk(ChunkRange range, IEnumerable<ILine> lines)
        {
            this.range = range;
            Lines = new List<ILine>(lines);
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