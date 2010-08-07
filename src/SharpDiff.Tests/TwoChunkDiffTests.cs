using System.Linq;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class TwoChunkDiffTests
    {
        const string fileOneContent = "one\r\ntwo\r\nthree\r\nfour\r\nfive\r\nsix\r\nseven\r\neight\r\nnine\r\nten";
        const string fileTwoContent = "zero\r\none\r\ntwo\r\nthree\r\nfour\r\nfive\r\nsix\r\nseven\r\neight\r\neight.5\r\nnine\r\nten";

        [Test]
        public void should_have_two_chunks()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);

            Assert.AreEqual(2, diff.Chunks.Count);
        }

        [Test]
        public void first_chunk_should_have_correct_new_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(1, chunk.NewRange.StartLine);
            Assert.AreEqual(4, chunk.NewRange.LinesAffected);
        }

        [Test]
        public void first_chunk_should_have_correct_old_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(1, chunk.OriginalRange.StartLine);
            Assert.AreEqual(3, chunk.OriginalRange.LinesAffected);
        }

        [Test]
        public void first_chunk_should_have_correct_number_of_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(2, chunk.Snippets.Count());
        }

        [Test]
        public void first_chunk_should_have_correct_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();
            var snippets = chunk.Snippets;

            var addition = snippets.ElementAtOrDefault(0) as AdditionSnippet;

            Assert.IsNotNull(addition);
            Assert.AreEqual(0, addition.OriginalLines.Count());
            Assert.AreEqual(1, addition.ModifiedLines.Count());
            Assert.AreEqual("zero", addition.ModifiedLines.First().Value);

            var context = snippets.ElementAtOrDefault(1) as ContextSnippet;

            Assert.IsNotNull(context);
            Assert.AreEqual(3, context.OriginalLines.Count());
            Assert.AreEqual("one", context.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("two", context.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual("three", context.OriginalLines.ElementAt(2).Value);
            Assert.AreEqual(0, context.ModifiedLines.Count());
        }

        [Test]
        public void second_chunk_should_have_correct_new_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.ElementAt(1);

            Assert.AreEqual(7, chunk.NewRange.StartLine);
            Assert.AreEqual(6, chunk.NewRange.LinesAffected);
        }

        [Test]
        public void second_chunk_should_have_correct_old_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.ElementAt(1);

            Assert.AreEqual(6, chunk.OriginalRange.StartLine);
            Assert.AreEqual(5, chunk.OriginalRange.LinesAffected);
        }

        [Test]
        public void second_chunk_should_have_correct_number_of_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.ElementAt(1);

            Assert.AreEqual(3, chunk.Snippets.Count());
        }

        [Test]
        public void second_chunk_should_have_correct_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.ElementAt(1);
            var snippets = chunk.Snippets;

            var firstContext = snippets.ElementAtOrDefault(0) as ContextSnippet;

            Assert.IsNotNull(firstContext);
            Assert.AreEqual(3, firstContext.OriginalLines.Count());
            Assert.AreEqual("six", firstContext.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("seven", firstContext.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual("eight", firstContext.OriginalLines.ElementAt(2).Value);
            Assert.AreEqual(0, firstContext.ModifiedLines.Count());

            var addition = snippets.ElementAtOrDefault(1) as AdditionSnippet;

            Assert.IsNotNull(addition);
            Assert.AreEqual(0, addition.OriginalLines.Count());
            Assert.AreEqual(1, addition.ModifiedLines.Count());
            Assert.AreEqual("eight.5", addition.ModifiedLines.ElementAt(0).Value);

            var secondContext = snippets.ElementAtOrDefault(2) as ContextSnippet;

            Assert.IsNotNull(secondContext);
            Assert.AreEqual(2, secondContext.OriginalLines.Count());
            Assert.AreEqual("nine", secondContext.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("ten", secondContext.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual(0, secondContext.ModifiedLines.Count());
        }
    }
}
