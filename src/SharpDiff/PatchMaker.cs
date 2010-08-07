using System;
using System.Collections.Generic;
using System.Linq;
using DiffMatchPatch;
using Chunk = DiffMatchPatch.Patch;
using Line = DiffMatchPatch.Diff;

namespace SharpDiff
{
    public class PatchMaker
    {
        public IEnumerable<DiffMatchPatch.Patch> MakePatch(string contentOne, string contentTwo, CompareOptions options)
        {
            var diff = new my.utils.Diff();
            var changes = diff.DiffText(contentOne, contentTwo);
            var contentOneLines = contentOne.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var contentTwoLines = contentTwo.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var chunks = new List<Chunk>();

            for (var i = 0; i < changes.Length; i++)
            {
                var change = changes[i];
                var nextChange = changes.ElementAtOrDefault(i + 1);

                var chunk = new Chunk();

                chunk.start1 = change.StartA;
                chunk.start2 = change.StartB;
                chunk.length1 = change.deletedA + options.ContextSize;
                chunk.length2 = change.insertedB + options.ContextSize;
                chunk.diffs = new List<Line>();
                chunks.Add(chunk);

                if (change.StartA != 0)
                {
                    // no start context needed
                    chunk.start1 = change.StartA - options.ContextSize;
                    chunk.start2 = change.StartB - options.ContextSize;
                    chunk.length1 = options.ContextSize + change.deletedA + options.ContextSize;
                    chunk.length2 = options.ContextSize + change.insertedB + options.ContextSize;

                    if (chunk.start1 + chunk.length1 > contentOneLines.Length)
                        chunk.length1 = chunk.length1 - ((chunk.start1 + chunk.length1) - contentOneLines.Length);

                    if (chunk.start2 + chunk.length2 > contentTwoLines.Length)
                        chunk.length2 = chunk.length2 - ((chunk.start2 + chunk.length2) - contentTwoLines.Length);

                    // stick some context in
                    var start = change.StartB - options.ContextSize;
                    for (var j = start; j < change.StartB; j++)
                    {
                        chunk.diffs.Add(new Line(Operation.EQUAL, contentTwoLines[j]));
                    }
                }

                for (int j = 0; j < change.insertedB; j++)
                {
                    var line = contentTwoLines[j + change.StartB];
                    chunk.diffs.Add(new Line(Operation.INSERT, line));
                }

                //if (distance_between(change, nextChange) > options.ContextSize * 2)
                //{
                    // need to split the diff into multiple chunks
                    var start2 = change.StartB + change.insertedB;
                    for (var j = start2; j < Math.Min(start2 + options.ContextSize, contentTwoLines.Length); j++)
                    {
                        chunk.diffs.Add(new Line(Operation.EQUAL, contentTwoLines[j]));
                    }
                //}
            }

            return chunks;
        }

        private static int distance_between(my.utils.Diff.Item change, my.utils.Diff.Item nextChange)
        {
            return nextChange.StartB - change.StartB + change.insertedB;
        }
    }
}