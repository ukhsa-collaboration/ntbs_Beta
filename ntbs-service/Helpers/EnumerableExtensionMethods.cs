using System;
using System.Collections.Generic;
using System.Linq;

namespace ntbs_service.Helpers
{
    internal static class EnumerableExtensionMethods
    {
        // Based on https://stackoverflow.com/a/8983717/2363767
        public static IEnumerable<List<T>> GroupByConsecutive<T>(this IEnumerable<T> source,
            Func<T, T, bool> prevNextPredicate)
        {
            var currentGroup = new List<T>();

            foreach (var item in source)
            {
                if (!currentGroup.Any() || prevNextPredicate(currentGroup.Last(), item))
                    currentGroup.Add(item); // Append: empty group or nearby elements.
                else
                {
                    // The group is done: yield it out
                    // and create a fresh group with the item.
                    yield return currentGroup;
                    currentGroup = new List<T> { item };
                }
            }

            // If the group still has items once the source is fully consumed,
            // we need to yield it out.
            if (currentGroup.Any())
                yield return currentGroup;
        }
    }
}
