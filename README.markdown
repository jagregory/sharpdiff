SharpDiff - Diff Parsing in .NET
================================

By [James Gregory](mailto:james@jagregory.com) [http://jagregory.com](http://jagregory.com)

About SharpDiff
---------------

SharpDiff is a library for parsing the output of various diffing tools. It's primary purpose is to reduce the time spent by SCM UI developers in handing diff output.

Why SharpDiff
-------------

I've got a few tools in mind that require parsing of diff files. I figure it's a pretty common thing for SCM UI developers to have to do, so I thought i'd put it out for others to use.

Parsing your first diff
-----------------------

The implementation is not concrete yet, but the current (easiest) way to parse a diff file is as follows.

	string diffContent = File.ReadAllText("MyDiffFile.diff");
	
	Diff diff = Diff.CreateFrom(diffContent);

From there you have a compiled version of your diff document. Intellisense will be your friend here, but basically you have a Chunks collection, and a Files collection.

The Wikipedia [article on Diffs](http://en.wikipedia.org/wiki/Diff) is worth a read if you're interested. In short though, chunks are formed as a chunk header containing the affected lines, and the lines themselves.

	@@ -1,3 +1,6 @@
	This is a small text file
 	+that I quite like,
 	 with a few lines of text
	-inside, nothing much.

The chunk header is **@@ -1,3 +1,6 @@**. The -1,3 describes the affected lines in the original file, the first number (ignoring the minus) is the start line, and the second number is the total of context lines plus subtraction lines. The second range (+1,6) is in the new file, and that starts on the first line, and has six affected lines; in this case it's all the context lines plus all addition lines.

All the other lines are the actual changes themselves. A line prefixed with a + is an addition line, a line prefixed with a - is a subtraction line, and lines prefixed with a space are context lines. Context lines are used for aligning the changes in the document. They're also useful for determining if the document has changed since the diff was created.

In the context of SharpDiff, your Chunks collection contains an instance of a Chunk for every part in the diff that resembles the above. Each chunk has the range info for the original and new file, and a Lines collection of each line.

Early Days
----------
**WARNING:** SharpDiff is still in early development. I wouldn't recommend using it in production code yet, as the parser is severely limited, and there's next to no error handling.

The biggest flaw is that it only supports the standard git-diff output, and doesn't handle many special circumstances.

It does support:

 * Standard git-diff header
 * Index extended header
 * Chunk header
 * Chunk lines (added, removed, and contextual)
 * No newline at end of file
 * Multiple chunks per diff
 * Multiple diff per input string

It doesn't support:

 * Other extended headers
 * Formats other than git-diff


Get Involved
------------
Download and try it out. If you're interested in helping with SharpDiff, then let me know. 
All comments, suggestions, and contributions are welcome. Feel free to contact me either through github, or my e-mail.

SharpDiff has a [Lighthouse dashboard](http://sharpdiff.lighthouseapp.com/projects/18529-sharpdiff/overview) where you can see any tickets and milestones that are outstanding. You're also welcome to add to that as needed.