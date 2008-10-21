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

        protected T Parse<T>(string text, Func<GitNumstatParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).As<T>();
        }
    }
}
