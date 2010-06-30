using System.Collections.Generic;

namespace SharpDiff.FileStructure
{
    public interface ISnippet
    {
        IEnumerable<ILine> OriginalLines { get; }
        IEnumerable<ILine> ModifiedLines { get; }
    }
}