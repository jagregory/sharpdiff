using System;

namespace SharpDiff
{
    public class InvalidDiffFormatException : Exception
    {
        public InvalidDiffFormatException()
            : base("Invalid diff format supplied.")
        {}
    }
}