using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public BomMode BomMode { get; set; }

        public CompareOptions()
        {
            ContextSize = 3;
            BomMode = BomMode.Include;
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
            if (fileOneContent == null && fileTwoContent == null)
                throw new InvalidOperationException("Both files were null");

            if (options.BomMode == BomMode.Ignore)
            {
                if (fileOneContent != null)
                    fileOneContent = RemoveBom(fileOneContent);
                if (fileTwoContent != null)
                    fileTwoContent = RemoveBom(fileTwoContent);
            }

            if (fileTwoContent == null)
                return DeletedFileDiff(fileOneContent, fileOnePath);
            if (fileOneContent == null)
                return NewFileDiff(fileTwoContent, fileTwoPath);
            if (IsBinary(fileOneContent))
                throw new BinaryFileException(fileOnePath);
            if (IsBinary(fileTwoContent))
                throw new BinaryFileException(fileTwoPath);

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

        static bool IsBinary(string content)
        {
            // todo: make this more robust
            return content.Contains("\0\0\0");
        }

        private static Diff DeletedFileDiff(string content, string path)
        {
            var header = new Header(new FormatType("generated"), new[]
            {
                new File('a', path),
                new File('b', "/dev/null")
            });

            var lines = content.SplitIntoLines()
                .Select(x => (ILine)new SubtractionLine(x));
            var range = new ChunkRange(new ChangeRange(1, lines.Count()), new ChangeRange(0, 0));
            var snippet = new SubtractionSnippet(lines);
            var chunk = new Chunk(range, new[] { snippet });

            return new Diff(header, new[] { chunk });
        }

        private static Diff NewFileDiff(string content, string path)
        {
            var header = new Header(new FormatType("generated"), new[]
            {
                new File('a', "/dev/null"),
                new File('b', path)
            });

            var lines = content.SplitIntoLines()
                .Select(x => (ILine)new AdditionLine(x));
            var range = new ChunkRange(new ChangeRange(0, 0), new ChangeRange(1, lines.Count()));
            var snippet = new AdditionSnippet(lines);
            var chunk = new Chunk(range, new[] { snippet });

            return new Diff(header, new[] { chunk });
        }

        private static readonly List<byte[]> ByteOrderMarks = new List<byte[]>
        {
            new byte[] { 0x00, 0x00, 0xFE, 0xFF },
            new byte[] { 0xFF, 0xFE, 0x00, 0x00 },
            new byte[] { 0xEF, 0xBB, 0xBF },
            new byte[] { 0xFE, 0xFF },
            new byte[] { 0xFF, 0xFE },
        };

        private static string RemoveBom(string content)
        {
            var encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(content);

            if (bytes.Take(4).ContainsOnly(ByteOrderMarks[0]) || bytes.Take(4).ContainsOnly(ByteOrderMarks[1]))
                return encoding.GetString(bytes.Skip(4).ToArray());

            if (bytes.Take(3).ContainsOnly(ByteOrderMarks[2]))
                return encoding.GetString(bytes.Skip(3).ToArray());

            if (bytes.Take(2).ContainsOnly(ByteOrderMarks[3]) || bytes.Take(2).ContainsOnly(ByteOrderMarks[4])) 
                return encoding.GetString(bytes.Skip(2).ToArray());

            return content;
        }
    }
}