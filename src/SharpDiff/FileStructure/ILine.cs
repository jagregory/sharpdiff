using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public interface ILine
    {
        string Value { get; }
        IEnumerable<Span> Spans { get; }
    }
}