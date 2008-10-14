namespace SharpDiff
{
    public class ChunkHeader
    {
        public ChunkHeader(FileDef originalFile, FileDef newFile)
        {
            this.OriginalFile = originalFile;
            this.NewFile = newFile;
        }

        public FileDef OriginalFile { get; private set; }
        public FileDef NewFile { get; private set; }
    }
}