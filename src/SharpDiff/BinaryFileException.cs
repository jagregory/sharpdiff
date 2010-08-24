using System;

namespace SharpDiff
{
    public class BinaryFileException : Exception
    {
        public BinaryFileException(string path)
            : base("Could not compare binary file '" + path + "'")
        {}
    }
}