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
                        new ChunkRange(new ChangeRange(0, 0), new ChangeRange(1, 1)), new[]
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
                        new ChunkRange(new ChangeRange(0, 0), new ChangeRange(1, 2)), new[]
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

        [Test]
        public void TwoLineFileWithOneLineAddedAtTop()
        {
            var patch = new Patch(
                new Diff(null, new[]
                {
                    new Chunk(
                        new ChunkRange(new ChangeRange(1, 2), new ChangeRange(1, 3)), new ILine[]
                        {
                            new AdditionLine("A LINE!"),
                            new ContextLine("original first line"),
                            new ContextLine("original second line")
                        }), 
                })
            );

            // @@ -1,2 +1,3 @@
            // +A LINE!
            //  original first line
            //  original second line

            patch.File = new StubFileAccessor(
                "original first line\r\n" +
                "original second line\r\n");
            var output = patch.ApplyTo("fake path");

            Assert.That(output, Is.EqualTo(
                "A LINE!\r\n" +
                "original first line\r\n" +
                "original second line\r\n"));
        }

        [Test]
        public void TwoLineFileWithLastLineRemoved()
        {
            var patch = new Patch(
                new Diff(null, new[]
                {
                    new Chunk(
                        new ChunkRange(new ChangeRange(1, 2), new ChangeRange(1, 1)), new ILine[]
                        {
                            new ContextLine("hello"),
                            new SubtractionLine("there")
                        }), 
                })
            );

            // @@ -1,2 +1,1 @@
            //  hello
            // -there

            patch.File = new StubFileAccessor(
                "hello\r\n" +
                "there\r\n");
            var output = patch.ApplyTo("fake path");

            Assert.That(output, Is.EqualTo(
                "hello\r\n"));
        }

        [Test]
        public void TwoLineFileWithBothLinesRemoved()
        {
            var patch = new Patch(
                new Diff(null, new[]
                {
                    new Chunk(
                        new ChunkRange(new ChangeRange(1, 2), new ChangeRange(0, 0)), new ILine[]
                        {
                            new SubtractionLine("hello"),
                            new SubtractionLine("there")
                        }), 
                })
            );

            // @@ -1,2 +0,0 @@
            // -hello
            // -there

            patch.File = new StubFileAccessor(
                "hello\r\n" +
                "there");
            var output = patch.ApplyTo("fake path");

            Assert.That(output, Is.EqualTo(""));
        }

        [Test]
        public void AdditionsAndRemovalsInSingleFile()
        {
            var patch = new Patch(
                new Diff(null, new[]
                {
                    new Chunk(
                        new ChunkRange(new ChangeRange(3, 9), new ChangeRange(3, 12)), new ILine[]
                        {
                            new ContextLine("this"),
                            new ContextLine("is"),
                            new ContextLine("a"),
                            new AdditionLine("here"),
                            new AdditionLine("are"),
                            new ContextLine("load"),
                            new ContextLine("of"),
                            new SubtractionLine("new"),
                            new AdditionLine("some"),
                            new AdditionLine("additions"),
                            new ContextLine("lines"),
                            new ContextLine("for"),
                            new ContextLine("complicating")
                        }), 
                })
            );

            //@@ -3,9 +3,12 @@
            // this
            // is
            // a
            //+here
            //+are
            // load
            // of
            //-new
            //+some
            //+additions
            // lines
            // for
            // complicating

            patch.File = new StubFileAccessor(
                "hello\r\n" +
                "there\r\n" +
                "this\r\n" +
                "is\r\n" +
                "a\r\n" +
                "load\r\n" +
                "of\r\n" +
                "new\r\n" +
                "lines\r\n" +
                "for\r\n" +
                "complicating\r\n" +
                "matters\r\n");
            var output = patch.ApplyTo("fake path");

            Assert.That(output, Is.EqualTo(
                "hello\r\n" +
                "there\r\n" +
                "this\r\n" +
                "is\r\n" +
                "a\r\n" +
                "here\r\n" +
                "are\r\n" +
                "load\r\n" +
                "of\r\n" +
                "some\r\n" +
                "additions\r\n" +
                "lines\r\n" +
                "for\r\n" +
                "complicating\r\n" +
                "matters\r\n"));
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