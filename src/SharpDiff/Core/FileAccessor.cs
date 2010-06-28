using System.IO;
using SharpDiff.Core;

namespace SharpDiff.Core
{
    public class FileAccessor : IFileAccessor
    {
        public string ReadAll(string path)
        {
            return File.ReadAllText(path);
        }
    }
}