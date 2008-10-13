using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class SharpDiffTests
    {
        [Test]
        public void InvalidDiffFormatExceptionThrownIfADiffFileOtherThanGitIsGiven()
        {
            var parser = new DiffParser2();

            Assert.Throws<InvalidDiffFormatException>(() => parser.Parse("diff --not-git"));
        }

        [Test]
        public void FileHeadersAreParsed()
        {
            var parser = new DiffParser2();
            var fileDiff = parser.Parse("diff --git a/SmallTextFile.txt b/SmallTextFile.txt");

            Assert.That(fileDiff.LeftFileName, Is.EqualTo("SmallTextFile.txt"));
            Assert.That(fileDiff.RightFileName, Is.EqualTo("SmallTextFile.txt"));
        }

        [Test]
        public void RebuildParser()
        {
            new OMetaDiffParser().something();
        }

        [Test]
        public void Test()
        {
            new OMetaDiffParser().Somethingelse();
        }
    }
}
