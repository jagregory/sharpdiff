namespace SharpDiff.FileStructure
{
    public class File : IFile
    {
        public File(char letter, string filename)
        {
            Letter = letter;
            FileName = filename;
        }

        public char Letter { get; private set; }
        public string FileName { get; private set; }
    }
}