using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public class AdditionSnippet : ISnippet
    {
        private readonly List<ILine> lines;

        public AdditionSnippet()
        {
            lines = new List<ILine>();
        }

        public AdditionSnippet(IEnumerable<ILine> lines)
        {
            this.lines = new List<ILine>(lines);
        }

        public void AddLine(ILine line)
        {
            lines.Add(line);
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