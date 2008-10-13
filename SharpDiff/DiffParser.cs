namespace SharpDiff
{
    public class DiffParser2
    {
        public FileDiff Parse(string diffText)
        {
            GuardAgainstInvalidDiffFormat(diffText);

            var diff = new FileDiff();

            diff.LeftFileName = GetFileName("a", diffText);
            diff.RightFileName = GetFileName("b", diffText);

            return diff;
        }

        private string GetFileName(string label, string text)
        {
            return "";
        }

        private void GuardAgainstInvalidDiffFormat(string text)
        {
            if (!text.StartsWith("diff --git "))
                throw new InvalidDiffFormatException();
        }
    }
}