using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class FormatTests : AbstractParserTestFixture
    {
        [Test]
        public void FormatParsed()
        {
            var result = Parse<FormatType>("--git", x => x.FormatType);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("git"));
        }
    }
}