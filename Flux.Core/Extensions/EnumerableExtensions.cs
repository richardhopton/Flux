using System.Linq;

// ReSharper disable CheckNamespace
namespace System.Collections.Generic
// ReSharper restore CheckNamespace
{
    public static class EnumerableExtensions
    {
        public static Boolean SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T,T,Boolean> predicate)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            using (IEnumerator<T> enumerator = first.GetEnumerator())
            {
                using (IEnumerator<T> enumerator2 = second.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (!enumerator2.MoveNext() || !predicate(enumerator.Current, enumerator2.Current))
                        {
                            return false;
                        }
                    }
                    if (enumerator2.MoveNext())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Boolean StartsWith<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, Boolean> predicate)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            first = first.ToArray();
            second = second.ToArray();
            return first.Count() >= second.Count() && first.Take(second.Count()).SequenceEqual(second, predicate);
        }

        public static Boolean EndsWith<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, Boolean> predicate)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            first = first.ToArray();
            second = second.ToArray();
            return first.Count() >= second.Count() && first.Reverse().Take(second.Count()).SequenceEqual(second.Reverse(), predicate);
        }

        public static Boolean Contains<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, Boolean> predicate)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            first = first.ToArray();
            second = second.ToArray();
            foreach (var value in second)
            {
                if (!first.Any(operand => predicate(operand, value)))
                {
                    return false;
                };
            }
            return true;
        }
    }
}
