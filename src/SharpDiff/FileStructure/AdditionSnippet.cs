using System.Collections.Generic;
using System.Linq;

namespace SharpDiff.FileStructure
{
    public class AdditionSnippet : ISnippet
    {
        private readonly ILine[] lines;

        public AdditionSnippet(IEnumerable<ILine> lines)
        {
            this.lines = lines.ToArray();
        }

        public IEnumerable<ILine> OriginalLines
        {
            get { yield break; }
        }

        public IEnumerable<ILine> ModifiedLines
        {
            get { return lines; }
        }
    }
}