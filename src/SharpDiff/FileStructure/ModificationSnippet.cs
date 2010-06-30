using System;
using System.Collections.Generic;
using System.Linq;
using DiffMatchPatch;

namespace SharpDiff.FileStructure
{
    public class ModificationSnippet : ISnippet
    {
        private readonly List<ModificationLine> originalLines = new List<ModificationLine>();
        private readonly List<ModificationLine> modifiedLines = new List<ModificationLine>();

        public ModificationSnippet(IEnumerable<ILine> originalLines, IEnumerable<ILine> modifiedLines)
        {
            CreateInlineDiffs(originalLines, modifiedLines);
        }

        private void CreateInlineDiffs(IEnumerable<ILine> originals, IEnumerable<ILine> modifieds)
        {
            var maxLines = Math.Max(originals.Count(), modifieds.Count());

            for (var i = 0; i < maxLines; i++)
            {
                var originalLine = originals.ElementAtOrDefault(i);
                var modifiedLine = modifieds.ElementAtOrDefault(i);

                if (originalLine == null && modifiedLine == null)
                    continue;
                if (originalLine != null && modifiedLine == null)
                    originalLines.Add(new ModificationLine(new[] { new Span(originalLine.Value, SpanKind.Deletion) }));
                else if (originalLine == null)
                    modifiedLines.Add(new ModificationLine(new[] { new Span(modifiedLine.Value, SpanKind.Addition) }));
                else
                {
                    var originalToModifiedChanges = DiffInline(originalLine, modifiedLine);

                    originalLines.Add(new ModificationLine(new[] { new Span(originalLine.Value, SpanKind.Equal) }));
                    modifiedLines.Add(new ModificationLine(originalToModifiedChanges));
                }
            }
        }

        static IEnumerable<Span> DiffInline(ILine originalLine, ILine modifiedLine)
        {
            var dmp = new diff_match_patch();
            var diffs = dmp.diff_main(originalLine.Value, modifiedLine.Value);

            dmp.diff_cleanupSemantic(diffs);

            return diffs
                .Select(x => new Span(x.text, OperationToKind(x.operation)))
                .ToArray();
        }

        static SpanKind OperationToKind(Operation operation)
        {
            switch (operation)
            {
                default:
                    return SpanKind.Equal;
                case Operation.INSERT:
                    return SpanKind.Addition;
                case Operation.DELETE:
                    return SpanKind.Deletion;
            }
        }

        public IEnumerable<ILine> OriginalLines
        {
            get { return originalLines.Cast<ILine>(); }
        }

        public IEnumerable<ILine> ModifiedLines
        {
            get { return modifiedLines.Cast<ILine>(); }
        }
    }
}