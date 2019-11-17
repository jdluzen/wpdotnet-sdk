using System.Collections.Generic;

namespace ClassLibrary1
{
    public static class Extensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> e)
        {
            while (e.MoveNext())
            {
                yield return e.Current;
            }
        }
    }
}
