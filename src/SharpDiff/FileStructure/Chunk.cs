using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public class Chunk
    {
        private readonly ChunkRange range;

        public Chunk(ChunkRange range, IEnumerable<ISnippet> snippets)
        {
            this.range = range;
            Snippets = new List<ISnippet>(snippets);
        }

        public ChangeRange OriginalRange
        {
            get { return range.OriginalRange; }
        }

        public ChangeRange NewRange
        {
            get { return range.NewRange; }
        }

        public IEnumerable<ISnippet> Snippets { get; private set; }
    }
}