using System;
using System.Collections.Generic;

namespace SharpDiff
{
    public class Header
    {
        public Header(FormatType format, FileDef file)
        {
            Format = format;
            File = file;
        }

        public FormatType Format { get; private set; }
        public FileDef File { get; private set; }

        public override string ToString()
        {
            var output = "Header: " + Format.Name + Environment.NewLine;

            //foreach (var file in Files)
            //{
                output += "  " + File.Letter + "/" + File.FileName + Environment.NewLine;
            //}

            return output;
        }
    }
}