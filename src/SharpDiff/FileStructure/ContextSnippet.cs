using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public class ContextSnippet : ISnippet
    {
        private readonly List<ILine> lines;

        public ContextSnippet(IEnumerable<ILine> lines)
        {
            this.lines = new List<ILine>(lines);
        }

        public IEnumerable<ILine> OriginalLines
        {
            get { return lines; }
        }

        public IEnumerable<ILine> ModifiedLines
        {
            get { yield break; }
        }
    }
}