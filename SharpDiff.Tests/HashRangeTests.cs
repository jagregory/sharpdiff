using NUnit.Framework;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class HashRangeTests : AbstractParserTestFixture
    {
        [Test]
        public void HashRangeParsed()
        {
            var result = Parse<HashRange>("c750789..f1c2d64", x => x.HashRange);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Start, Is.EqualTo("c750789"));
            Assert.That(result.End, Is.EqualTo("f1c2d64"));
        }
    }
}