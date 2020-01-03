using System.Collections;
using System.Collections.Generic;

namespace ntbs_service.Helpers
{
    /// <summary>
    /// An IEnumerator cannot be used in a `foreach` loop directly.
    /// Instead the loop needs an object that implements a `GetEnumerator` method
    /// </summary>
    public static class EnumeratorExtensions
    {
        public static IEnumerable ToIEnumerable(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
