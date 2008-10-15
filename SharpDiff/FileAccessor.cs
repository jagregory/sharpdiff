using System.IO;

namespace SharpDiff
{
    public class FileAccessor : IFileAccessor
    {
        public string ReadAll(string path)
        {
            return File.ReadAllText(path);
        }
    }
}