using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpDiff.FileStructure;

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

            Assert.That(result.Snippets, Is.Not.Null);
            Assert.That(result.Snippets, Has.Count.EqualTo(3));
            Assert.That(result.Snippets.ElementAt(0).OriginalLines.First(), Is.TypeOf<ContextLine>());
            Assert.That(result.Snippets.ElementAt(1).ModifiedLines.First(), Is.TypeOf<AdditionLine>());
            Assert.That(result.Snippets.ElementAt(2).OriginalLines.First(), Is.TypeOf<SubtractionLine>());
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
                " public class Tests {\r\n" +
                "+        [Test]\r\n" +
                "@@ -26,5 +24,15 @@ namespace SharpDiff\r\n" +
                "         [Test]\r\n" +
                "-            var result = Parse<ChangeRange>(\"-1,30\", x => x.ChangeRange);\r\n", x => x.Chunks);

            var list = new List<Chunk>(result);

            Assert.That(list.Count, Is.EqualTo(2));

            var firstChunk = list[0];
            var secondChunk = list[1];

            // there are two lines per chunk, we should ignore the extra data after the range
            Assert.That(firstChunk.Snippets, Has.Count.EqualTo(2));
            Assert.That(firstChunk.Snippets.ElementAt(0).OriginalLines.First(), Is.TypeOf<ContextLine>());
            Assert.That(firstChunk.Snippets.ElementAt(1).ModifiedLines.First(), Is.TypeOf<AdditionLine>());
            Assert.That(secondChunk.Snippets, Has.Count.EqualTo(2));
            Assert.That(secondChunk.Snippets.ElementAt(0).OriginalLines.First(), Is.TypeOf<ContextLine>());
            Assert.That(secondChunk.Snippets.ElementAt(1).OriginalLines.First(), Is.TypeOf<SubtractionLine>());
        }
    }
}