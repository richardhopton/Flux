using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class Extensions
    {
        public static Boolean ContainedWithin<T>(this T item, params T[] items)
        {
            return items.Contains(item);
        }

        public static Boolean ContainedWithin<T>(this T item, IEnumerable<T> items)
        {
            return items.Contains(item);
        }
    }
}
