using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDiff.FileStructure.Numstat
{
    public class FileStats
    {
        public FileStats(int additions, int subtractions, string filename)
        {
            Additions = additions;
            Subtractions = subtractions;
            Filename = filename;
        }

        public int Additions { get; private set; }
        public int Subtractions { get; private set; }
        public string Filename { get; private set; }
    }
}
