using System;
using System.Linq.Expressions;

namespace Flux.Core
{
    public static class Requires
    {
        public static void NotNull<T>(T value, String parameterName) where T : class
        {
            if (value == null)
            {
                throw new NullReferenceException(String.Format("'{0}' is required", parameterName));
            }
        }

        public static void NotNull<T>(T value, Expression<Func<T>> parameter) where T : class
        {
            NotNull(value, parameter.GetName());
        }
    }
}
