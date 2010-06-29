using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace SharpDiff.Tests
{
    public static class IListExtensions
    {
        public static IEnumerable<T> AssertItem<T>(this IEnumerable<T> list, int index, ConstraintExpression constraint)
        {
            list.AssertItem(index, (item) => Assert.That(item, constraint));

            return list;
        }

        public static IEnumerable<T> AssertItem<T>(this IEnumerable<T> list, int index, Action<T> assertionAction)
        {
            Assert.That(list, Is.Not.Null, "List was null, cannot access item at index '" + index + "'.");
            Assert.That(list.Count() - 1, Is.GreaterThanOrEqualTo(index), "Index '" + index + "' was out of range.");

            var item = list.ElementAt(index);

            assertionAction(item);

            return list;
        }
    }
}