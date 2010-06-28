using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class DiffTests : AbstractParserTestFixture
    {
        [Test]
        public void DiffAndFormatParsed()
        {
            var result = Parse<Header>("diff --git", x => x.Header);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Format, Is.Not.Null);
            Assert.That(result.Format.Name, Is.EqualTo("git"));
        }

        [Test]
        public void DiffAndFormatParsedWithFiles()
        {
            var result = Parse<Header>("diff --git a/Filename b/File2.txt", x => x.Header);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Format, Is.Not.Null);
            Assert.That(result.Format.Name, Is.EqualTo("git"));

            var list = new List<IFile>(result.Files);
            var first = (File)list[0];
            var second = (File)list[1];

            Assert.That(result, Is.Not.Null);
            Assert.That(first, Is.Not.Null);
            Assert.That(first.Letter, Is.EqualTo('a'));
            Assert.That(first.FileName, Is.EqualTo("Filename"));

            Assert.That(second, Is.Not.Null);
            Assert.That(second.Letter, Is.EqualTo('b'));
            Assert.That(second.FileName, Is.EqualTo("File2.txt"));
        }

        [Test]
        public void TwoDiffs()
        {
            var result = ParseList<Diff>(
                "diff --git a/SmallTextFile.txt b/SmallTextFile.txt\r\n" +
                "index f1c2d64..c750789 100644\r\n" +
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -1,3 +1,4 @@\r\n" +
                " This is a small text file\r\n" +
                "+of my almighty creation,\r\n" +
                " with a few lines of text\r\n" +
                " inside, nothing much.\r\n" +
                "diff --git a/SmallTextFile.txt b/SmallTextFile.txt\r\n" +
                "index f1c2d64..c750789 100644\r\n" +
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -1,3 +1,4 @@\r\n" +
                " This is a small text file\r\n" +
                "+of my almighty creation,\r\n" +
                " with a few lines of text\r\n" +
                " inside, nothing much.\r\n", x => x.Diffs);

            Assert.That(result, Is.Not.Null);

            var list = new List<Diff>(result);
            Assert.That(list.Count, Is.EqualTo(2));
        }

        [Test]
        public void IsDeletionIsFalseWhenBothFilenamesArePresent()
        {
            var result = Parse<Header>("diff --git a/code.cs b/code.cs\r\n", x => x.Header);

            Assert.That(result.IsDeletion, Is.False);
        }

        [Test]
        public void IsDeletionIsTrueWhenRightFileIsNull()
        {
            var result = Parse<Header>("diff --git a/code.cs /dev/null\r\n", x => x.Header);

            Assert.That(result.IsDeletion, Is.True);
        }

        [Test]
        public void IsNewFileIsFalseWhenBothFilenamesArePresent()
        {
            var result = Parse<Header>("diff --git a/code.cs b/code.cs\r\n", x => x.Header);

            Assert.That(result.IsNewFile, Is.False);
        }

        [Test]
        public void IsNewFileIsTrueWhenLeftFileIsNull()
        {
            var result = Parse<Header>("diff --git /dev/null b/code.cs\r\n", x => x.Header);

            Assert.That(result.IsNewFile, Is.True);
        }

        [Test]
        public void ShowMeTheMoney()
        {
            var result = Parse<Diff>(
                "diff --git a/SmallTextFile.txt b/SmallTextFile.txt\r\n" +
                "index f1c2d64..c750789 100644\r\n" +
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -1,3 +1,4 @@\r\n" +
                " This is a small text file\r\n" +
                "+of my almighty creation,\r\n" +
                " with a few lines of text\r\n" +
                " inside, nothing much.\r\n", x => x.Diff);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void LargerFile()
        {
            var result = Parse<Diff>(
                "diff --git a/SmallTextFile.txt b/SmallTextFile.txt\r\n" +
                "index f1c2d64..a59864c 100644\r\n" +
                "--- a/SmallTextFile.txt\r\n" +
                "+++ b/SmallTextFile.txt\r\n" +
                "@@ -1,3 +1,6 @@\r\n" +
                " This is a small text file\r\n" +
                "+that I quite like,\r\n" +
                " with a few lines of text\r\n" +
                "-inside, nothing much.\r\n" +
                "\\ No newline at end of file\r\n" +
                "+inside, nothing much.\r\n" +
                "+\r\n" +
                "+You like it, right?\r\n", x => x.Diff);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void TwoChunkFile()
        {
            var result = Parse<Diff>(
                "diff --git a/code.cs b/code.cs\r\n" +
                "index 9610949..45fb865 100644\r\n" +
                "--- a/code.cs\r\n" +
                "+++ b/code.cs\r\n" +
                "@@ -11,8 +11,6 @@ namespace SharpDiff\r\n" +
                "             var contents = File.ReadAllText(\"Parser\\GitDiffParser.ometacs\");\r\n" +
                "             var result = Grammars.ParseGrammarThenOptimizeThenTranslate\r\n" +
                "                 <OMetaParser, OMetaOptimizer, OMetaTranslator>();\r\n" +
                "-\r\n" +
                "-            File.WriteAllText(@\"C:\\Development\\SharpDiff\\SharpDiff\\Parser\\GitDiffParser.cs\", result);\r\n" +
                "         }\r\n" +
                "         \r\n" +
                "         [Test]\r\n" +
                "@@ -26,5 +24,15 @@ namespace SharpDiff\r\n" +
                "             Assert.That(result.NewRange.StartLine, Is.EqualTo(1));\r\n" +
                "             Assert.That(result.NewRange.LinesAffected, Is.EqualTo(3));\r\n" +
                "         }\r\n" +
                "+		\r\n" +
                "+		        [Test]\r\n" +
                "+        public void OriginalChangeRangeParsed()\r\n" +
                "+        {\r\n" +
                "+            var result = Parse<ChangeRange>(\"-1,30\", x => x.ChangeRange);\r\n" +
                "+\r\n" +
                "+            Assert.That(result, Is.Not.Null);\r\n" +
                "+            Assert.That(result.StartLine, Is.EqualTo(1));\r\n" +
                "+            Assert.That(result.LinesAffected, Is.EqualTo(30));\r\n" +
                "+        }\r\n" +
                "     }\r\n" +
                " }\r\n" +
                "\\ No newline at end of file\r\n", x => x.Diff);

            Assert.That(result, Is.Not.Null);
        }
    }
}
