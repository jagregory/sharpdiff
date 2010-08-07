using System.Collections.Generic;
using System.Linq;
using DiffMatchPatch;
using OMetaSharp;
using SharpDiff.FileStructure;
using SharpDiff.Parsers;
using Diff = SharpDiff.FileStructure.Diff;

namespace SharpDiff
{
    public class CompareOptions
    {
        public int ContextSize { get; set; }

        public CompareOptions()
        {
            ContextSize = 3;
        }
    }

    public static class Differ
    {
        public static IEnumerable<Diff> Load(string diffContent)
        {
            return new List<Diff>(Grammars.ParseWith<GitDiffParser>(diffContent, x => x.Diffs).ToIEnumerable<Diff>());
        }

        public static Diff Compare(string fileOnePath, string fileOneContent, string fileTwoPath, string fileTwoContent)
        {
            return Compare(fileOnePath, fileOneContent, fileTwoPath, fileTwoContent, new CompareOptions());
        }

        public static Diff Compare(string fileOnePath, string fileOneContent, string fileTwoPath, string fileTwoContent, CompareOptions options)
        {
            var patchMaker = new PatchMaker();
            var patches = patchMaker.MakePatch(fileOneContent, fileTwoContent, options);
            var chunks = new List<Chunk>();

            foreach (var patch in patches)
            {
                var originalRange = new ChangeRange(patch.start1 + 1, patch.length1);
                var newRange = new ChangeRange(patch.start2 + 1, patch.length2);
                var range = new ChunkRange(originalRange, newRange);
                var snippets = new List<ISnippet>();

                var lines = new List<DiffMatchPatch.Diff>();
                Operation? previousOperation = null;
                var isModification = false;

                foreach (var diff in patch.diffs)
                {
                    if (previousOperation == null)
                        previousOperation = diff.operation;
                    if (previousOperation == Operation.DELETE && diff.operation == Operation.INSERT)
                        isModification = true;
                    else if (previousOperation != diff.operation)
                    {
                        // different operation
                        if (previousOperation == Operation.EQUAL)
                            snippets.Add(new ContextSnippet(lines.Select(x => new ContextLine(x.text)).Cast<ILine>()));
                        else if (isModification)
                            snippets.Add(new ModificationSnippet(
                                lines
                                    .Where(x => x.operation == Operation.DELETE)
                                    .Select(x => new SubtractionLine(x.text))
                                    .Cast<ILine>(),
                                lines
                                    .Where(x => x.operation == Operation.INSERT)
                                    .Select(x => new AdditionLine(x.text))
                                    .Cast<ILine>()
                            ));
                        else if (previousOperation == Operation.INSERT)
                            snippets.Add(new AdditionSnippet(lines.Select(x => new AdditionLine(x.text)).Cast<ILine>()));
                        else if (previousOperation == Operation.DELETE)
                            snippets.Add(new SubtractionSnippet(lines.Select(x => new SubtractionLine(x.text)).Cast<ILine>()));

                        lines.Clear();
                        isModification = false;
                    }

                    lines.Add(diff);
                    previousOperation = diff.operation;
                }

                if (lines.Count > 0)
                {
                    if (previousOperation == Operation.INSERT)
                        snippets.Add(new AdditionSnippet(lines.Select(x => new AdditionLine(x.text)).Cast<ILine>()));
                    else if (previousOperation == Operation.DELETE)
                        snippets.Add(new SubtractionSnippet(lines.Select(x => new SubtractionLine(x.text)).Cast<ILine>()));
                    else
                        snippets.Add(new ContextSnippet(lines.Select(x => new ContextLine(x.text)).Cast<ILine>()));
                }

                chunks.Add(new Chunk(range, snippets));
            }

            var header = new Header(new FormatType("generated"), new[]
            {
                new File('a', fileOnePath),
                new File('b', fileTwoPath)
            });

            return new Diff(header, chunks);
        }

        static string ReadFile(string path)
        {
            return System.IO.File.ReadAllText(path.Replace("/", "\\"));
        }

       
    }
}