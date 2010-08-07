using System.Linq;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class OneChunkDiffTests
    {
        const string fileOneContent = "one\r\ntwo\r\nthree\r\nfour\r\nfive\r\nsix\r\nseven\r\neight\r\nnine\r\nten";
        const string fileTwoContent = "zero\r\none\r\ntwo\r\nthree\r\nthree.5\r\nfour\r\nfive\r\nsix\r\nseven\r\neight\r\nnine\r\nten";

        [Test]
        public void should_have_one_chunk()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);

            Assert.AreEqual(1, diff.Chunks.Count);
        }

        [Test]
        public void chunk_should_have_correct_new_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(1, chunk.NewRange.StartLine);
            Assert.AreEqual(8, chunk.NewRange.LinesAffected);
        }

        [Test]
        public void chunk_should_have_correct_old_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(1, chunk.OriginalRange.StartLine);
            Assert.AreEqual(6, chunk.OriginalRange.LinesAffected);
        }

        [Test]
        public void chunk_should_have_correct_number_of_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(4, chunk.Snippets.Count());
        }

        [Test]
        public void chunk_should_have_correct_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();
            var snippets = chunk.Snippets;

            var firstAddition = snippets.ElementAtOrDefault(0) as AdditionSnippet;

            Assert.IsNotNull(firstAddition);
            Assert.AreEqual(0, firstAddition.OriginalLines.Count());
            Assert.AreEqual(1, firstAddition.ModifiedLines.Count());
            Assert.AreEqual("zero", firstAddition.ModifiedLines.First().Value);

            var firstContext = snippets.ElementAtOrDefault(1) as ContextSnippet;

            Assert.IsNotNull(firstContext);
            Assert.AreEqual(3, firstContext.OriginalLines.Count());
            Assert.AreEqual("one", firstContext.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("two", firstContext.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual("three", firstContext.OriginalLines.ElementAt(2).Value);
            Assert.AreEqual(0, firstContext.ModifiedLines.Count());

            var secondAddition = snippets.ElementAtOrDefault(2) as AdditionSnippet;

            Assert.IsNotNull(secondAddition);
            Assert.AreEqual(0, secondAddition.OriginalLines.Count());
            Assert.AreEqual(1, secondAddition.ModifiedLines.Count());
            Assert.AreEqual("three.5", secondAddition.ModifiedLines.First().Value);

            var secondContext = snippets.ElementAtOrDefault(3) as ContextSnippet;

            Assert.IsNotNull(secondContext);
            Assert.AreEqual(3, secondContext.OriginalLines.Count());
            Assert.AreEqual("four", secondContext.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("five", secondContext.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual("six", secondContext.OriginalLines.ElementAt(2).Value);
            Assert.AreEqual(0, secondContext.ModifiedLines.Count());
        }
    }
}