using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpDiff.FileStructure
{
    public interface ISnippet
    {
        IEnumerable<ILine> OriginalLines { get; }
        IEnumerable<ILine> ModifiedLines { get; }
    }

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

    public class SubtractionSnippet : ISnippet
    {
        private readonly IEnumerable<ILine> lines;

        public SubtractionSnippet(IEnumerable<ILine> lines)
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

    public class ModifiedSnippet : ISnippet
    {
        private readonly ReadOnlyCollection<ILine> originalLines;
        private readonly ReadOnlyCollection<ILine> modifiedLines;

        public ModifiedSnippet(List<ILine> originalLines, List<ILine> modifiedLines)
        {
            this.originalLines = originalLines.AsReadOnly();
            this.modifiedLines = modifiedLines.AsReadOnly();
        }

        public IEnumerable<ILine> OriginalLines
        {
            get { return originalLines; }
        }

        public IEnumerable<ILine> ModifiedLines
        {
            get { return modifiedLines; }
        }
    }

    public class Chunk
    {
        private readonly ChunkRange range;

        public Chunk(ChunkRange range, IEnumerable<ISnippet> snippets)
        {
            this.range = range;
            Snippets = new List<ISnippet>(snippets);
        }

        public ChangeRange OriginalRange
        {
            get { return range.OriginalRange; }
        }

        public ChangeRange NewRange
        {
            get { return range.NewRange; }
        }

        public IEnumerable<ISnippet> Snippets { get; private set; }
    }
}