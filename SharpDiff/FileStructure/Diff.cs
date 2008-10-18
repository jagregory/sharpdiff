using System.Collections.Generic;
using OMetaSharp;
using SharpDiff.FileStructure;

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

        public IList<FileDef> Files
        {
            get { return header.Files; }
        }

        public static IList<Diff> CreateFrom(string content)
        {
            return new List<Diff>(Grammars.ParseWith<GitDiffParser>(content, x => x.Diffs).ToIEnumerable<Diff>());
        }
    }
}