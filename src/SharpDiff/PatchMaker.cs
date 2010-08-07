using System;
using System.Collections.Generic;
using System.Linq;
using DiffMatchPatch;
using Chunk = DiffMatchPatch.Patch;
using Diff = my.utils.Diff;
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

            var chunks = new HashSet<Chunk>();
            Chunk currentChunk = null;

            for (var i = 0; i < changes.Length; i++)
            {
                var change = changes[i];
                var nextChange = changes.ElementAtOrDefault(i + 1);
                var continuation = currentChunk != null;

                if (!continuation)
                {
                    currentChunk = CreateChunk(change, options);
                    chunks.Add(currentChunk);
                }

                if (change.StartA != 0 && !continuation)
                {
                    // no start context needed
                    currentChunk.start1 = change.StartA - options.ContextSize;
                    currentChunk.start2 = change.StartB - options.ContextSize;

                    // stick some context in
                    var start = change.StartB - options.ContextSize;
                    for (var j = start; j < change.StartB; j++)
                    {
                        currentChunk.diffs.Add(new Line(Operation.EQUAL, contentTwoLines[j]));
                    }
                }

                for (var j = 0; j < change.insertedB; j++)
                {
                    var line = contentTwoLines[j + change.StartB];
                    currentChunk.diffs.Add(new Line(Operation.INSERT, line));
                }

                var start2 = change.StartB + change.insertedB;
                for (var j = start2; j < Math.Min(start2 + options.ContextSize, contentTwoLines.Length); j++)
                {
                    currentChunk.diffs.Add(new Line(Operation.EQUAL, contentTwoLines[j]));
                }

                if (distance_between(change, nextChange) > options.ContextSize * 2)
                {
                    // need to split the diff into multiple chunks
                    currentChunk = null;
                }
            }

            foreach (var chunk in chunks)
            {
                chunk.length1 = chunk.diffs.Count(x => x.operation == Operation.EQUAL || x.operation == Operation.DELETE);
                chunk.length2 = chunk.diffs.Count(x => x.operation == Operation.EQUAL || x.operation == Operation.INSERT);
            }

            return chunks;
        }

        private Chunk CreateChunk(Diff.Item change, CompareOptions options)
        {
            var chunk = new Chunk();
            chunk.start1 = change.StartA;
            chunk.start2 = change.StartB;
            chunk.length1 = change.deletedA + options.ContextSize;
            chunk.length2 = change.insertedB + options.ContextSize;
            chunk.diffs = new List<Line>();
            return chunk;
        }

        private static int distance_between(my.utils.Diff.Item change, my.utils.Diff.Item nextChange)
        {
            return nextChange.StartB - change.StartB + change.insertedB;
        }
    }
}