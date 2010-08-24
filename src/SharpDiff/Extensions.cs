using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDiff
{
    internal static class Extensions
    {
        public static bool ContainsOnly<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            if (source == null || other == null)
                return false;
            if (source.Count() != other.Count())
                return false;

            var sourceEnumerator = source.GetEnumerator();
            var otherEnumerator = other.GetEnumerator();

            while (sourceEnumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                if (!sourceEnumerator.Current.Equals(otherEnumerator.Current))
                    return false;
            }

            return true;
        }

        public static string[] SplitIntoLines(this string value)
        {
            return value.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
        }
    }
}