using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OMetaSharp;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class SharpDiffTests
    {
        [Test, Explicit]
        public void RebuildParser()
        {
            new OMetaCodeGenerator().Rebuild();
        }

        [Test]
        public void FormatParsed()
        {
            var result = Parse<FormatType>("--git", x => x.FormatType);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("git"));
        }

        [Test]
        public void FilenameParsed()
        {
            var result = Parse<FileDef>("a/File.name", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("File.name"));
        }

        [Test]
        public void FilenameParsedWithoutExtension()
        {
            var result = Parse<FileDef>("a/Filename", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("Filename"));
        }

        [Test]
        public void FilenameParsedWithPreceedingSpace()
        {
            var result = Parse<FileDef>(" a/Filename", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("Filename"));
        }

        [Test]
        public void MultipleFilenamesAreParsed()
        {
            var result = ParseList<FileDef>(" a/Filename b/Second.txt", x => x.FileDefs);
            var list = new List<FileDef>(result);

            Assert.That(result, Is.Not.Null);
            Assert.That(list[0], Is.Not.Null);
            Assert.That(list[0].Letter, Is.EqualTo('a'));
            Assert.That(list[0].FileName, Is.EqualTo("Filename"));

            Assert.That(list[1], Is.Not.Null);
            Assert.That(list[1].Letter, Is.EqualTo('b'));
            Assert.That(list[1].FileName, Is.EqualTo("Second.txt"));
        }

        [Test]
        public void DiffAndFormatParsed()
        {
            var result = Parse<Header>("diff --git", x => x.Header);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Format, Is.Not.Null);
            Assert.That(result.Format.Name, Is.EqualTo("git"));
        }

        [Test]
        public void DiffAndFormatParsedWithFiles()
        {
            var result = Parse<Header>("diff --git a/Filename b/File2.txt", x => x.Header);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Format, Is.Not.Null);
            Assert.That(result.Format.Name, Is.EqualTo("git"));

            var list = new List<FileDef>(result.Files);

            Assert.That(result, Is.Not.Null);
            Assert.That(list[0], Is.Not.Null);
            Assert.That(list[0].Letter, Is.EqualTo('a'));
            Assert.That(list[0].FileName, Is.EqualTo("Filename"));

            Assert.That(list[1], Is.Not.Null);
            Assert.That(list[1].Letter, Is.EqualTo('b'));
            Assert.That(list[1].FileName, Is.EqualTo("File2.txt"));
        }

        [Test]
        public void HashRangeParsed()
        {
            var result = Parse<HashRange>("c750789..f1c2d64", x => x.HashRange);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Start, Is.EqualTo("c750789"));
            Assert.That(result.End, Is.EqualTo("f1c2d64"));
        }

        [Test]
        public void IndexExtendedHeaderIsParsed()
        {
            var result = Parse<IndexHeader>("index c750789..f1c2d64 100644", x => x.IndexHeader);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Range, Is.Not.Null);
            Assert.That(result.Range.Start, Is.EqualTo("c750789"));
            Assert.That(result.Range.End, Is.EqualTo("f1c2d64"));
            Assert.That(result.Mode, Is.EqualTo(100644));
        }

        [Test]
        public void ChunkHeaderReturnedForAddRemoveFileHeader()
        {
            var result = Parse<ChunkHeader>("--- a/SmallTextFile.txt\r\n+++ b/SmallTextFile.txt", x => x.ChunkHeader);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.OriginalFile.Letter, Is.EqualTo('a'));
            Assert.That(result.OriginalFile.FileName, Is.EqualTo("SmallTextFile.txt"));
            Assert.That(result.NewFile.Letter, Is.EqualTo('b'));
            Assert.That(result.NewFile.FileName, Is.EqualTo("SmallTextFile.txt"));
        }

        [Test]
        public void ChunkReturnedWithHeader()
        {
            var result = Parse<Chunk>("--- a/SmallTextFile.txt\r\n+++ b/SmallTextFile.txt\r\n@@ -1,30 +1,3 @@", x => x.Chunk);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.OriginalFile.Letter, Is.EqualTo('a'));
            Assert.That(result.OriginalFile.FileName, Is.EqualTo("SmallTextFile.txt"));
            Assert.That(result.NewFile.Letter, Is.EqualTo('b'));
            Assert.That(result.NewFile.FileName, Is.EqualTo("SmallTextFile.txt"));
        }

        [Test]
        public void OriginalChangeRangeParsed()
        {
            var result = Parse<ChangeRange>("-1,30", x => x.ChangeRange);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StartLine, Is.EqualTo(1));
            Assert.That(result.LinesAffected, Is.EqualTo(30));
        }

        [Test]
        public void NewChangeRangeParsed()
        {
            var result = Parse<ChangeRange>("+1,3", x => x.ChangeRange);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StartLine, Is.EqualTo(1));
            Assert.That(result.LinesAffected, Is.EqualTo(3));
        }
        
        [Test]
        public void ChunkRangeParsed()
        {
            var result = Parse<ChunkRange>("@@ -1,30 +1,3 @@", x => x.ChunkRange);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.OriginalRange.StartLine, Is.EqualTo(1));
            Assert.That(result.OriginalRange.LinesAffected, Is.EqualTo(30));
            Assert.That(result.NewRange.StartLine, Is.EqualTo(1));
            Assert.That(result.NewRange.LinesAffected, Is.EqualTo(3));
        }

        [Test]
        public void ChunkHasChunkRange()
        {
            var result = Parse<Chunk>("--- a/SmallTextFile.txt\r\n+++ b/SmallTextFile.txt\r\n@@ -1,30 +1,3 @@", x => x.Chunk);

            Assert.That(result.OriginalRange.StartLine, Is.EqualTo(1));
            Assert.That(result.OriginalRange.LinesAffected, Is.EqualTo(30));
            Assert.That(result.NewRange.StartLine, Is.EqualTo(1));
            Assert.That(result.NewRange.LinesAffected, Is.EqualTo(3));
        }

        [Test]
        public void LinePrefixedWithASpaceIsAContextLine()
        {
            ILine result = Parse<ContextLine>(" This is a context line\r\n", x => x.ContextLine);

            Assert.That(result.Value, Is.EqualTo("This is a context line"));
        }

        [Test]
        public void LinePrefixedWithAPlusIsAnAdditionLine()
        {
            ILine result = Parse<AdditionLine>("+This is an addition line\r\n", x => x.AdditionLine);

            Assert.That(result.Value, Is.EqualTo("This is an addition line"));
        }

        [Test]
        public void LinePrefixedWithASpaceIsASubtractionLine()
        {
            ILine result = Parse<SubtractionLine>("-This is a subtraction line\r\n", x => x.SubtractionLine);

            Assert.That(result.Value, Is.EqualTo("This is a subtraction line"));
        }

        [Test]
        public void NoNewLineAtEOFLineParsed()
        {
            ILine result = Parse<NoNewLineAtEOFLine>("\\ No newline at end of file\r\n", x => x.NoNewLineAtEOFLine);

            Assert.That(result.Value, Is.EqualTo("No newline at end of file"));
        }

        [Test]
        public void MultipleLinesParsed()
        {
            var result = ParseList<ILine>(
                " This is a small text file\r\n" +
                "+that I quite like,\r\n" +
                " with a few lines of text\r\n" +
                "-inside, nothing much.\r\n" +
                "\\ No newline at end of file\r\n" +
                "+inside, nothing much.\r\n", x => x.DiffLines);

            new List<ILine>(result)
                .AssertItem(0, Is.TypeOf<ContextLine>())
                .AssertItem(1, Is.TypeOf<AdditionLine>())
                .AssertItem(2, Is.TypeOf<ContextLine>())
                .AssertItem(3, Is.TypeOf<SubtractionLine>())
                .AssertItem(4, Is.TypeOf<NoNewLineAtEOFLine>())
                .AssertItem(5, Is.TypeOf<AdditionLine>());
        }

        [Test]
        public void ChunkReturnedWithLines()
        {
            var result = Parse<Chunk>(
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -1,30 +1,3 @@\r\n" +
                " This is a context line\r\n" +
                "+This is an addition line\r\n" +
                "-This is a subtraction line\r\n", x => x.Chunk);

            Assert.That(result.Lines, Is.Not.Null);
            Assert.That(result.Lines[0], Is.TypeOf<ContextLine>());
            Assert.That(result.Lines[1], Is.TypeOf<AdditionLine>());
            Assert.That(result.Lines[2], Is.TypeOf<SubtractionLine>());
        }

        [Test, Explicit]
        public void ShowMeTheMoney()
        {
            var result = Parse<Diff>(
                "diff --git a/SmallTextFile.txt b/SmallTextFile.txt\r\n" +
                "index f1c2d64..c750789 100644\r\n" +
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -1,3 +1,4 @@\r\n" +
                " This is a small text file\r\n" +
                "+of my almighty creation,\r\n" +
                " with a few lines of text\r\n" +
                " inside, nothing much.\r\n", x => x.Diff);

            Assert.That(result, Is.Not.Null);
        }

        [Test, Explicit]
        public void LargerFile()
        {
            var result = Parse<Diff>(
                "diff --git a/SmallTextFile.txt b/SmallTextFile.txt\r\n" +
                "index f1c2d64..a59864c 100644\r\n" +
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -1,3 +1,6 @@\r\n" +
                " This is a small text file\r\n" +
                "+that I quite like,\r\n" +
                " with a few lines of text\r\n" +
                "-inside, nothing much.\r\n" +
                "\\ No newline at end of file\r\n" +
                "+inside, nothing much.\r\n" +
                "+\r\n" +
                "+You like it, right?\r\n", x => x.Diff);

            Assert.That(result, Is.Not.Null);
        }

        private T Parse<T>(string text, Func<DiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).As<T>();
        }

        private IEnumerable<T> ParseList<T>(string text, Func<DiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).ToIEnumerable<T>();
        }
    }
}
