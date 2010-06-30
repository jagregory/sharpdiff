using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public class AdditionLine : ILine
    {
        public AdditionLine(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public IEnumerable<Span> Spans
        {
            get { return new[] { new Span(Value, SpanKind.Addition) }; }
        }
    }
}