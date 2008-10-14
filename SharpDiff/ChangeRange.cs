namespace SharpDiff
{
    public class ChangeRange
    {
        public ChangeRange(int startLine, int linesAffected)
        {
            StartLine = startLine;
            LinesAffected = linesAffected;
        }
        public int StartLine { get; private set; }
        public int LinesAffected { get; private set; }
    }
}