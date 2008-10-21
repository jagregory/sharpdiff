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
            var result = Parse<FileStats>("3\t8\tanotherFile.txt", x => x.FileStats);

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
            var result = Parse<FileStats>("3\t8\tanotherFile.txt", x => x.FileStats);

            Assert.That(result.Filename, Is.EqualTo("anotherFile.txt"));
        }

        protected T Parse<T>(string text, Func<GitNumstatParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).As<T>();
        }
    }
}
