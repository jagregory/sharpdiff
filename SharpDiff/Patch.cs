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
                var insertLocation = chunk.NewRange.StartLine;

                foreach (var line in chunk.Lines)
                {
                    // this smells
                    if (line is AdditionLine)
                    {
                        fileLines.Insert(insertLocation, line.Value);
                        insertLocation++;
                    }
                }
            }

            return string.Join("\r\n", fileLines.ToArray());
        }
    }
}