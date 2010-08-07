using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public class Diff
    {
        private readonly Header header;

        public Diff(Header header, IEnumerable<Chunk> chunks)
        {
            this.header = header;
            Chunks = new List<Chunk>(chunks);
        }

        public IList<Chunk> Chunks { get; private set; }

        public IList<IFile> Files
        {
            get { return header.Files; }
        }

        public bool IsNewFile
        {
            get { return header.IsNewFile; }
        }

        public bool IsDeletion
        {
            get { return header.IsDeletion; }
        }
    }
}