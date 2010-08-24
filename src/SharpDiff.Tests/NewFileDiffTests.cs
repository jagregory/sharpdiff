using System.Linq;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class NewFileDiffTests
    {
        const string fileOneContent = null;
        const string fileTwoContent = "one";

        [Test]
        public void should_have_one_chunks()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);

            Assert.AreEqual(1, diff.Chunks.Count);
            Assert.IsInstanceOf<AdditionSnippet>(diff.Chunks.Single().Snippets.Single());
        }
    }
}