namespace SharpDiff.FileStructure
{
    public class ChunkHeader
    {
        public ChunkHeader(IFile originalFile, IFile newFile)
        {
            OriginalFile = originalFile;
            NewFile = newFile;
        }

        public IFile OriginalFile { get; private set; }
        public IFile NewFile { get; private set; }
    }
}