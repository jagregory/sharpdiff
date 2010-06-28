using System.Collections.Generic;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class FilenameTests : AbstractParserTestFixture
    {
        [Test]
        public void FilenameParsed()
        {
            var result = Parse<File>("a/File.name", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("File.name"));
        }

        [Test]
        public void FilenameParsedWithoutExtension()
        {
            var result = Parse<File>("a/Filename", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("Filename"));
        }

        [Test]
        public void FilenameParsedWithPreceedingSpace()
        {
            var result = Parse<File>(" a/Filename", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("Filename"));
        }

        [Test]
        public void MultipleFilenamesAreParsed()
        {
            var result = ParseList<File>(" a/Filename b/Second.txt", x => x.FileDefs);
            var list = new List<File>(result);

            Assert.That(result, Is.Not.Null);
            Assert.That(list[0], Is.Not.Null);
            Assert.That(list[0].Letter, Is.EqualTo('a'));
            Assert.That(list[0].FileName, Is.EqualTo("Filename"));

            Assert.That(list[1], Is.Not.Null);
            Assert.That(list[1].Letter, Is.EqualTo('b'));
            Assert.That(list[1].FileName, Is.EqualTo("Second.txt"));
        }

        [Test]
        public void FilenameWithPathIsParsed()
        {
            var result = Parse<File>("a/this/is/the/Filename.txt", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("this/is/the/Filename.txt"));
        }

        [Test]
        public void FilenameContainingFullStops()
        {
            var result = Parse<File>("a/this.is.the.Filename.txt", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Letter, Is.EqualTo('a'));
            Assert.That(result.FileName, Is.EqualTo("this.is.the.Filename.txt"));
        }

        [Test]
        public void DevNullParsed()
        {
            var result = Parse<IFile>("/dev/null", x => x.FileDef);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NullFile>());
            Assert.That(result.FileName, Is.EqualTo("/dev/null"));
        }
    }
}