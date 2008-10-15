namespace SharpDiff
{
    public interface IFileAccessor
    {
        string ReadAll(string path);
    }
}