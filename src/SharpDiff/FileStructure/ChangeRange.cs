namespace SharpDiff.FileStructure
{
    /// <summary>
    /// Represents the +/-1,3 in a ChunkRange header (@@ -1,3 +1,3 @@)
    /// </summary>
    public class ChangeRange
    {
        public ChangeRange(int startLine, int linesAffected)
        {
            StartLine = startLine;
            LinesAffected = linesAffected;
        }

        /// <summary>
        /// First line of change. Non-zero indexed, first line is 1.
        /// </summary>
        public int StartLine { get; private set; }
        
        /// <summary>
        /// Number of lines affected in the change. Non-zero indexed, one line addition equals 1 in this property.
        /// </summary>
        public int LinesAffected { get; private set; }
    }
}