using SharpDiff.FileStructure;

namespace SharpDiff.FileStructure
{
    public class IndexHeader
    {
        public IndexHeader(HashRange range, int mode)
        {
            Range = range;
            Mode = mode;
        }

        public HashRange Range { get; private set; }
        public int Mode { get; private set; }
    }
}