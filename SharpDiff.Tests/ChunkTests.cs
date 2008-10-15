using System.Collections.Generic;
using NUnit.Framework;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class ChunkTests : AbstractParserTestFixture
    {
        [Test]
        public void ChunkHasChunkRange()
        {
            var result = Parse<Chunk>(
                "@@ -1,30 +1,3 @@", x => x.Chunk);

            Assert.That(result.OriginalRange.StartLine, Is.EqualTo(1));
            Assert.That(result.OriginalRange.LinesAffected, Is.EqualTo(30));
            Assert.That(result.NewRange.StartLine, Is.EqualTo(1));
            Assert.That(result.NewRange.LinesAffected, Is.EqualTo(3));
        }

        [Test]
        public void ChunkReturnedWithLines()
        {
            var result = Parse<Chunk>(
                "@@ -1,30 +1,3 @@\r\n" +
                " This is a context line\r\n" +
                "+This is an addition line\r\n" +
                "-This is a subtraction line\r\n", x => x.Chunk);

            Assert.That(result.Lines, Is.Not.Null);
            Assert.That(result.Lines[0], Is.TypeOf<ContextLine>());
            Assert.That(result.Lines[1], Is.TypeOf<AdditionLine>());
            Assert.That(result.Lines[2], Is.TypeOf<SubtractionLine>());
        }

        [Test]
        public void MultipleChunksParsed()
        {
            var result = ParseList<Chunk>(
                "@@ -11,8 +11,6 @@ namespace SharpDiff\r\n" +
                "@@ -26,5 +24,15 @@ namespace SharpDiff\r\n", x => x.Chunks);

            var list = new List<Chunk>(result);

            Assert.That(list.Count, Is.EqualTo(2));

            var firstChunk = list[0];
            var secondChunk = list[1];

            Assert.That(firstChunk.OriginalRange.StartLine, Is.EqualTo(11));
            Assert.That(firstChunk.OriginalRange.LinesAffected, Is.EqualTo(8));
            Assert.That(firstChunk.NewRange.StartLine, Is.EqualTo(11));
            Assert.That(firstChunk.NewRange.LinesAffected, Is.EqualTo(6));
            Assert.That(secondChunk.OriginalRange.StartLine, Is.EqualTo(26));
            Assert.That(secondChunk.OriginalRange.LinesAffected, Is.EqualTo(5));
            Assert.That(secondChunk.NewRange.StartLine, Is.EqualTo(24));
            Assert.That(secondChunk.NewRange.LinesAffected, Is.EqualTo(15));
        }

        [Test]
        public void MultipleChunksLinesParsed()
        {
            var result = ParseList<Chunk>(
                "@@ -11,8 +11,6 @@ namespace SharpDiff\r\n" +
                "+        [Test]\r\n" +
                "@@ -26,5 +24,15 @@ namespace SharpDiff\r\n" +
                "-            var result = Parse<ChangeRange>(\"-1,30\", x => x.ChangeRange);\r\n", x => x.Chunks);

            var list = new List<Chunk>(result);

            Assert.That(list.Count, Is.EqualTo(2));

            var firstChunk = list[0];
            var secondChunk = list[1];

            // there are two lines per chunk, the first is appended to the range!
            Assert.That(firstChunk.Lines.Count, Is.EqualTo(2));
            Assert.That(firstChunk.Lines[0], Is.TypeOf<ContextLine>());
            Assert.That(firstChunk.Lines[1], Is.TypeOf<AdditionLine>());
            Assert.That(secondChunk.Lines.Count, Is.EqualTo(2));
            Assert.That(secondChunk.Lines[0], Is.TypeOf<ContextLine>());
            Assert.That(secondChunk.Lines[1], Is.TypeOf<SubtractionLine>());
        }
    }
}