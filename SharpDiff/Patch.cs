using System;
using System.Collections.Generic;
using SharpDiff.Core;
using SharpDiff.FileStructure;

namespace SharpDiff
{
    public class Patch
    {
        private readonly Diff diff;
        public IFileAccessor File { get; set;}

        public Patch(Diff diff)
        {
            this.diff = diff;
            File = new FileAccessor();
        }

        public string ApplyTo(string path)
        {
            var fileContents = File.ReadAll(path);
            var fileLines = new List<string>(fileContents.Split(new[] {"\r\n"}, StringSplitOptions.None));

            foreach (var chunk in diff.Chunks)
            {
                var lineLocation = chunk.NewRange.StartLine - 1; // zero-index the start line 

                if (lineLocation < 0)
                    lineLocation = 0;

                foreach (var line in chunk.Lines)
                {
                    // this smells
                    if (line is AdditionLine)
                        fileLines.Insert(lineLocation, line.Value);
                    if (line is SubtractionLine)
                    {
                        fileLines.RemoveAt(lineLocation);
                        continue;
                    }

                    lineLocation++;
                }
            }

            return string.Join("\r\n", fileLines.ToArray());
        }
    }
}