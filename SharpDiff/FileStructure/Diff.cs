using System.Collections.Generic;
using SharpDiff.FileStructure;

namespace SharpDiff.FileStructure
{
    public class Diff
    {
        private Header header;

        public Diff(Header header, IEnumerable<Chunk> chunks)
        {
            this.header = header;
            Chunks = new List<Chunk>(chunks);
        }

        public IList<Chunk> Chunks { get; private set; }
    }
}