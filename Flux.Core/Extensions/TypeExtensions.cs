using System.Text;
using Flux.Core;

// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class TypeExtensions
    {
        public static Boolean IsNumericType(this Type type)
        {
            return TypeHelper.IsImplicitNumericConversion(type, typeof(Double));
        }

        public static String GetFriendlyName(this Type type, Boolean includeNamespaces = true)
        {
            var typeName = type.Name;
            if (includeNamespaces)
            {
                typeName = String.Format("{0}.{1}", type.Namespace, type.Name);
            }
            if (type.IsGenericType)
            {
                var genericArgs = type.GetGenericArguments();
                var sb = new StringBuilder(typeName.Replace(String.Format("`{0}", genericArgs.Length), "<"));
                foreach (var subType in type.GetGenericArguments())
                {
                    if (sb.Length > typeName.Length)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(subType.GetFriendlyName(includeNamespaces));
                }
                sb.Append(">");
                return sb.ToString();
            }
            return typeName;
        }
    }
}
