namespace SharpDiff.FileStructure
{
    public class ChunkHeader
    {
        public ChunkHeader(FileDef originalFile, FileDef newFile)
        {
            OriginalFile = originalFile;
            NewFile = newFile;
        }

        public FileDef OriginalFile { get; private set; }
        public FileDef NewFile { get; private set; }
    }
}