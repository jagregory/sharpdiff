namespace SharpDiff.FileStructure
{
    public class NullFile : IFile
    {
        public string FileName
        {
            get { return "/dev/null"; }
        }
    }
}