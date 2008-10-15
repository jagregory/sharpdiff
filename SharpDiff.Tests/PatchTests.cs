using NUnit.Framework;
using SharpDiff.Core;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class PatchTests
    {
        [Test]
        public void EmptyFileWithOneAdditionReturnsTheOneLine()
        {
            var patch = new Patch(
                new Diff(null, new[]
                {
                    new Chunk(
                        new ChunkRange(new ChangeRange(0, 0), new ChangeRange(0, 1)), new[]
                        {
                            new AdditionLine("A LINE!")
                        }), 
                })
            );

            patch.File = new StubFileAccessor("");
            var output = patch.ApplyTo("fake path");

            Assert.That(output, Is.EqualTo("A LINE!\r\n")); // didn't specify that it shouldn't end in a newline
        }

        [Test]
        public void EmptyFileWithTwoAdditionReturnsBothLines()
        {
            var patch = new Patch(
                new Diff(null, new[]
                {
                    new Chunk(
                        new ChunkRange(new ChangeRange(0, 0), new ChangeRange(0, 1)), new[]
                        {
                            new AdditionLine("A LINE!"),
                            new AdditionLine("Another line!")
                        }), 
                })
            );

            patch.File = new StubFileAccessor("");
            var output = patch.ApplyTo("fake path");

            Assert.That(output, Is.EqualTo("A LINE!\r\nAnother line!\r\n")); // didn't specify that it shouldn't end in a newline
        }
    }

    internal class StubFileAccessor : IFileAccessor
    {
        private readonly string contents;

        public StubFileAccessor(string contents)
        {
            this.contents = contents;
        }

        public string ReadAll(string path)
        {
            return contents;
        }
    }
}