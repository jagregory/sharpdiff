using System.Linq;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class DeletedFileDiffTests
    {
        const string fileOneContent = "one";
        const string fileTwoContent = null;

        [Test]
        public void should_have_one_chunks()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);

            Assert.AreEqual(1, diff.Chunks.Count);
            Assert.IsInstanceOf<SubtractionSnippet>(diff.Chunks.Single().Snippets.Single());
        }
    }
}