using System;
using System.Collections.Generic;
using SharpDiff.FileStructure;

namespace SharpDiff.FileStructure
{
    public class Header
    {
        public Header(FormatType format)
            : this(format, new List<FileDef>())
        {}

        public Header(FormatType format, IEnumerable<FileDef> files)
        {
            Format = format;
            Files = new List<FileDef>(files);
        }

        public FormatType Format { get; private set; }
        public IList<FileDef> Files { get; private set; }

        public override string ToString()
        {
            var output = "Header: " + Format.Name + Environment.NewLine;

            foreach (var file in Files)
            {
                output += "  " + file.Letter + "/" + file.FileName + Environment.NewLine;
            }

            return output;
        }
    }
}