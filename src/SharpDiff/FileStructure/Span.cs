using System;

namespace SharpDiff.FileStructure
{
    public class Span
    {
        public Span(string value, SpanKind kind)
        {
            Value = value;
            Kind = kind;
        }

        public string Value { get; private set; }
        public SpanKind Kind { get; private set; }
    }
}