using System;
using System.Collections.Generic;
using OMetaSharp;

namespace SharpDiff.Tests
{
    public abstract class AbstractParserTestFixture
    {
        protected T Parse<T>(string text, Func<DiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).As<T>();
        }

        protected IEnumerable<T> ParseList<T>(string text, Func<DiffParser, Rule<char>> ruleFetcher)
        {
            return Grammars.ParseWith(text, ruleFetcher).ToIEnumerable<T>();
        }
    }
}