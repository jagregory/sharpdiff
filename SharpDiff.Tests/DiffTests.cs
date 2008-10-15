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

            var list = new List<FileDef>(result.Files);

            Assert.That(result, Is.Not.Null);
            Assert.That(list[0], Is.Not.Null);
            Assert.That(list[0].Letter, Is.EqualTo('a'));
            Assert.That(list[0].FileName, Is.EqualTo("Filename"));

            Assert.That(list[1], Is.Not.Null);
            Assert.That(list[1].Letter, Is.EqualTo('b'));
            Assert.That(list[1].FileName, Is.EqualTo("File2.txt"));
        }

        [Test, Explicit]
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

        [Test, Explicit]
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

        [Test, Explicit]
        public void TwoChunkFile()
        {
            var result = Parse<Diff>(
                "diff --git a/code.cs b/code.cs\r\n" +
                "index 9610949..45fb865 100644\r\n" +
                "--- a/code.cs\r\n" +
                "+++ b/code.cs\r\n" +
                "@@ -11,8 +11,6 @@ namespace SharpDiff\r\n" +
                "             var contents = File.ReadAllText(\"Parser\\DiffParser.ometacs\");\r\n" +
                "             var result = Grammars.ParseGrammarThenOptimizeThenTranslate\r\n" +
                "                 <OMetaParser, OMetaOptimizer, OMetaTranslator>();\r\n" +
                "-\r\n" +
                "-            File.WriteAllText(@\"C:\\Development\\SharpDiff\\SharpDiff\\Parser\\DiffParser.cs\", result);\r\n" +
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
