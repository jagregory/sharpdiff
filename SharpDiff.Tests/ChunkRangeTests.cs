using NUnit.Framework;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class ChunkRangeTests : AbstractParserTestFixture
    {
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
        public void CanParseChunkRangeWhenOnSameLineAsDiffLine()
        {
            var result = Parse<ChunkRange>("@@ -11,8 +11,6 @@ namespace SharpDiff\r\n", x => x.ChunkRange);

            Assert.That(result.OriginalRange.StartLine, Is.EqualTo(11));
            Assert.That(result.OriginalRange.LinesAffected, Is.EqualTo(8));
            Assert.That(result.NewRange.StartLine, Is.EqualTo(11));
            Assert.That(result.NewRange.LinesAffected, Is.EqualTo(6));
        }
    }
}