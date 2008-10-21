using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDiff.FileStructure.Numstat
{
    public class FileStats
    {
        public FileStats(int additions, int subtractions)
        {
            Additions = additions;
            Subtractions = subtractions;
        }

        public int Additions { get; private set; }
        public int Subtractions { get; private set; }
    }
}
