using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OMetaSharp;
using SharpDiff.FileStructure.Numstat;
using SharpDiff.Parsers;

namespace SharpDiff.Tests.Numstat
{
    [TestFixture]
    public class FileStatsTests
    {
        [Test]
        public void ParsesNumber()
        {
            var result = Parse<int>("1", x => x.Number);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ParsesAdditionsAndSubtractionValues()
        {
            var result = Parse<FileStats>("3\t8\tanotherFile.txt\r\n", x => x.FileStats);

            Assert.That(result.Additions, Is.EqualTo(3));
            Assert.That(result.Subtractions, Is.EqualTo(8));
        }

        [Test]
        public void ParsesFilename()
        {
            var result = Parse<string>("anotherFile.txt", x => x.Filename);

            Assert.That(result, Is.EqualTo("anotherFile.txt"));
        }

        [Test]
        public void ParsesFullFileLine()
        {
            var result = Parse<FileStats>("3\t8\tanotherFile.txt\r\n", x => x.FileStats);

            Assert.That(result.Filename, Is.EqualTo("anotherFile.txt"));
        }

        [Test]
        public void CanParseMultipleLines()
        {
            var result = ParseList<FileStats>(
                "3\t8\tfile.txt\r\n" +
                "5\t1\tanotherFile.txt\r\n", x => x.FullFile);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Filename, Is.EqualTo("file.txt"));
            Assert.That(result[0].Additions, Is.EqualTo(3));
            Assert.That(result[0].Subtractions, Is.EqualTo(8));
            Assert.That(result[1].Filename, Is.EqualTo("anotherFile.txt"));
            Assert.That(result[1].Additions, Is.EqualTo(5));
            Assert.That(result[1].Subtractions, Is.EqualTo(1));
        }

        protected T Parse<T>(string text, Func<GitNumstatParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).As<T>();
        }

        protected IList<T> ParseList<T>(string text, Func<GitNumstatParser, Rule<char>> ruleFetcher)
        {
            return new List<T>(Grammars.ParseWith(text, ruleFetcher).ToIEnumerable<T>());
        }
    }
}
