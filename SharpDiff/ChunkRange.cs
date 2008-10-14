namespace SharpDiff
{
    public class ChunkRange
    {
        public ChunkRange(ChangeRange originalRange, ChangeRange newRange)
        {
            OriginalRange = originalRange;
            NewRange = newRange;
        }

        public ChangeRange OriginalRange { get; private set; }
        public ChangeRange NewRange { get; private set; }
    }
}