namespace SharpDiff.Core
{
    public interface IFileAccessor
    {
        string ReadAll(string path);
    }
}