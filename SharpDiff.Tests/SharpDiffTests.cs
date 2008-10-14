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
            var result = Parse<Diff>("diff --git", x => x.Header);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Format, Is.Not.Null);
            Assert.That(result.Format.Name, Is.EqualTo("git"));
        }

        [Test]
        public void DiffAndFormatParsedWithFiles()
        {
            var result = Parse<Diff>("diff --git a/Filename b/File2.txt", x => x.Header);

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

        private T Parse<T>(string text, Func<DiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).As<T>();
        }

        private IEnumerable<T> ParseList<T>(string text, Func<DiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).ToIEnumerable<T>();
        }

        // @@ -1,3 +1,3 @@
        // @@ -L,N +L,N @@
        // L = Start line
        // N = Lines affected

        /*
Diff
  Format
  Files
  ChunkHeader
    OriginalRange
	NewRange
	Lines
		AdditionLine
		SubtractionLine
		ContextLine
         */

        /*
diff --git a/SmallTextFile.txt b/SmallTextFile.txt
index f1c2d64..c750789 100644
--- a/SmallTextFile.txt
+++ b/SmallTextFile.txt
@@ -1,3 +1,4 @@
 This is a small text file
+of my almighty creation,
 with a few lines of text
 inside, nothing much.
\ No newline at end of file
         */
    }
}
