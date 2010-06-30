using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public class NoNewLineAtEOFLine : ILine
    {
        public string Value
        {
            get { return "No newline at end of file"; }
        }

        public IEnumerable<Span> Spans
        {
            get { return new[] { new Span(Value, SpanKind.Equal) }; }
        }
    }
}