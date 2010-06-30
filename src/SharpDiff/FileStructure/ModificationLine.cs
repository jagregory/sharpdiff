using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDiff.FileStructure
{
    public class ModificationLine : ILine
    {
        public ModificationLine(IEnumerable<Span> spans)
        {
            Spans = spans;
        }

        public string Value
        {
            get { return String.Join("", Spans.Select(x => x.Value).ToArray()) ; }
        }

        public IEnumerable<Span> Spans { get; private set; }
    }
}