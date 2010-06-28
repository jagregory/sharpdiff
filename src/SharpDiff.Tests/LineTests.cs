using System.Collections.Generic;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class LineTests : AbstractParserTestFixture
    {
        [Test]
        public void CanParseLineWhenOnSameLineAsChunkRange()
        {
            var result = Parse<Diff>(
                "diff --git a/SmallTextFile.txt b/SmallTextFile.txt\r\n" +
                "index f1c2d64..a59864c 100644\r\n" +
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -11,8 +11,6 @@ namespace SharpDiff\r\n", x => x.Diff);

            var chunk = result.Chunks[0];

            Assert.That(chunk.OriginalRange.StartLine, Is.EqualTo(11));
            Assert.That(chunk.OriginalRange.LinesAffected, Is.EqualTo(8));
            Assert.That(chunk.NewRange.StartLine, Is.EqualTo(11));
            Assert.That(chunk.NewRange.LinesAffected, Is.EqualTo(6));

            chunk.Lines
                .AssertItem(0, Is.TypeOf<ContextLine>())
                .AssertItem(0, item => Assert.That(item.Value, Is.EqualTo("namespace SharpDiff")));
        }

        [Test]
        public void LinePrefixedWithASpaceIsAContextLine()
        {
            ILine result = Parse<ContextLine>(" This is a context line\r\n", x => x.ContextLine);

            Assert.That(result.Value, Is.EqualTo("This is a context line"));
        }

        [Test]
        public void EmptyContextLine()
        {
            ILine result = Parse<ContextLine>(" \r\n", x => x.ContextLine);

            Assert.That(result.Value, Is.EqualTo(""));
        }

        [Test]
        public void LinePrefixedWithAPlusIsAnAdditionLine()
        {
            ILine result = Parse<AdditionLine>("+This is an addition line\r\n", x => x.AdditionLine);

            Assert.That(result.Value, Is.EqualTo("This is an addition line"));
        }

        [Test]
        public void EmptyAdditionLine()
        {
            ILine result = Parse<AdditionLine>("+\r\n", x => x.AdditionLine);

            Assert.That(result.Value, Is.EqualTo(""));
        }

        [Test]
        public void LinePrefixedWithASpaceIsASubtractionLine()
        {
            ILine result = Parse<SubtractionLine>("-This is a subtraction line\r\n", x => x.SubtractionLine);

            Assert.That(result.Value, Is.EqualTo("This is a subtraction line"));
        }

        [Test]
        public void EmptySubtractionLine()
        {
            ILine result = Parse<SubtractionLine>("-\r\n", x => x.SubtractionLine);

            Assert.That(result.Value, Is.EqualTo(""));
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
    }
}