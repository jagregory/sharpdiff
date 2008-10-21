using System;
using System.Collections.Generic;
using OMetaSharp;
using SharpDiff.Parsers;

namespace SharpDiff.Tests
{
    public abstract class AbstractParserTestFixture
    {
        protected T Parse<T>(string text, Func<GitDiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).As<T>();
        }

        protected IEnumerable<T> ParseList<T>(string text, Func<GitDiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).ToIEnumerable<T>();
        }
    }
}