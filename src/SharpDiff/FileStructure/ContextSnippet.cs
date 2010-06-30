using System.Collections.Generic;
using System.Linq;

namespace SharpDiff.FileStructure
{
    public class ContextSnippet : ISnippet
    {
        private readonly IEnumerable<ILine> lines;

        public ContextSnippet(IEnumerable<ILine> lines)
        {
            this.lines = lines.ToArray();
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