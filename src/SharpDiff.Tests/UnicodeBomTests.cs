using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpDiff.FileStructure;
using File = System.IO.File;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class UnicodeBomTests
    {
        readonly string fileOneContent = read_text("Fixtures\\file_with_bom.txt");
        readonly string fileTwoContent = read_text("Fixtures\\file_without_bom.txt");

        [Test]
        public void when_compared_default_should_consider_bom_changes_as_significant()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);

            Assert.AreEqual(1, diff.Chunks.Count, "Should have only one chunk");

            var snippets = diff.Chunks.Single().Snippets;

            Assert.AreEqual(2, snippets.Count(), "Should have two snippets");
            Assert.IsInstanceOfType(typeof(ModificationSnippet), snippets.First());
            Assert.IsInstanceOfType(typeof(ContextSnippet), snippets.ElementAt(1));
        }

        [Test]
        public void when_compared_ignore_bom_should_consider_bom_changes_as_insignificant()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent, new CompareOptions
            {
                BomMode = BomMode.Ignore
            });

            Assert.AreEqual(0, diff.Chunks.Count, "Should have no chunks (identical)");
        }

        static string read_text(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var encoding = new UTF8Encoding();

            return encoding.GetString(bytes);
        }
    }
}