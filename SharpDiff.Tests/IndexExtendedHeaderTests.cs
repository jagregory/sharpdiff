using NUnit.Framework;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class IndexExtendedHeaderTests : AbstractParserTestFixture
    {
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
    }
}