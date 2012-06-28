using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Flux.Core
{
    public static class TypeHelper
    {
        public static bool AreReferenceTypesCompatible(Type sourceType, Type destinationType)
        {
            return (ReferenceEquals(sourceType, destinationType) ||
                    IsImplicitReferenceConversion(sourceType, destinationType));
        }

        public static bool AreTypesCompatible(object source, Type destinationType)
        {
            if (source != null)
            {
                return AreTypesCompatible(source.GetType(), destinationType);
            }
            return !destinationType.IsValueType || IsNullableType(destinationType);
        }

        public static bool AreTypesCompatible(Type sourceType, Type destinationType)
        {
            if (!ReferenceEquals(sourceType, destinationType) &&
                !IsImplicitNumericConversion(sourceType, destinationType) &&
                !IsImplicitReferenceConversion(sourceType, destinationType) &&
                !IsImplicitBoxingConversion(sourceType, destinationType))
            {
                return IsImplicitNullableConversion(sourceType, destinationType);
            }
            return true;
        }

        public static bool ContainsCompatibleType(Type targetType, params Type[] types)
        {
            return ContainsCompatibleType(types, targetType);
        }

        public static bool ContainsCompatibleType(IEnumerable<Type> enumerable, Type targetType)
        {
            return enumerable.Any(type => AreTypesCompatible(type, targetType));
        }

        public static T Convert<T>(object source)
        {
            T local;
            if (source is T)
            {
                return (T)source;
            }
            if (source == null)
            {
                if (typeof(T).IsValueType && !IsNullableType(typeof(T)))
                {
                    throw new InvalidCastException(String.Format("Cannot convert {0} to {1}", "null", typeof(T)));
                }
                return default(T);
            }
            if (typeof(T) == typeof(String))
            {
                var value = (Object)source.ToString();
                local = (T)value;
            }
            else if (!TryNumericConversion(source, out local))
            {
                throw new InvalidCastException(String.Format("Cannot convert {0} to {1}", source, typeof(T)));
            }
            return local;
        }

        public static Object Convert(object source, Type destinationType)
        {
            Object local;
            if (source == null)
            {
                if (destinationType.IsValueType && !IsNullableType(destinationType))
                {
                    throw new InvalidCastException(String.Format("Cannot convert {0} to {1}", "null", destinationType));
                }
                return null;
            }
            var sourceType = source.GetType();
            if (IsImplicitReferenceConversion(sourceType, destinationType))
            {
                return source;
            }
            if (!TryNumericConversion(source, destinationType, out local))
            {
                throw new InvalidCastException(String.Format("Cannot convert {0} to {1}", source, destinationType));
            }
            return local;
        }

        public static IEnumerable<Type> GetCompatibleTypes(IEnumerable<Type> enumerable, Type targetType)
        {
            return enumerable.Where(type => AreTypesCompatible(type, targetType));
        }

        public static object GetDefaultValueForType(Type type)
        {
            if (!type.IsValueType ||
                IsNullableValueType(type))
            {
                return null;
            }
            if (type.IsEnum)
            {
                var values = Enum.GetValues(type);
                if (values.Length > 0)
                {
                    return values.GetValue(0);
                }
            }
            return Activator.CreateInstance(type);
        }

        public static IEnumerable<Type> GetImplementedTypes(Type type)
        {
            var typesEncountered = new HashSet<Type>();
            GetImplementedTypesHelper(type, typesEncountered);
            return typesEncountered;
        }

        private static void GetImplementedTypesHelper(Type type, ISet<Type> typesEncountered)
        {
            if (typesEncountered.Contains(type))
                return;
            typesEncountered.Add(type);
            var types = type.GetInterfaces();
            foreach (var t in types)
            {
                GetImplementedTypesHelper(t, typesEncountered);
            }
            for (var type2 = type.BaseType; (type2 != null) && (type2 != typeof(Object)); type2 = type2.BaseType)
            {
                GetImplementedTypesHelper(type2, typesEncountered);
            }
        }

        public static bool IsImplicitBoxingConversion(Type sourceType, Type destinationType)
        {
            return ((sourceType.IsValueType &&
                     ((destinationType == typeof(Object)) ||
                      (destinationType == typeof(ValueType)))) ||
                    (sourceType.IsEnum &&
                     (destinationType == typeof(Enum))));
        }

        public static bool IsImplicitNullableConversion(Type sourceType, Type destinationType)
        {
            if (!IsNullableType(destinationType))
            {
                return false;
            }
            destinationType = destinationType.GetGenericArguments()[0];
            if (IsNullableType(sourceType))
            {
                sourceType = sourceType.GetGenericArguments()[0];
            }
            return AreTypesCompatible(sourceType, destinationType);
        }

        public static bool IsImplicitNumericConversion(Type source, Type target)
        {
            var sourceTypeCode = Type.GetTypeCode(source);
            var targetTypeCode = Type.GetTypeCode(target);
            switch (sourceTypeCode)
            {
                case TypeCode.Byte:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    }
                case TypeCode.SByte:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    }
                case TypeCode.Int16:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    }
                case TypeCode.Int32:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    }
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }
                        break;
                    }
                case TypeCode.Single:
                    if (targetTypeCode != TypeCode.Double)
                    {
                        break;
                    }
                    return true;
            }
            return false;
        }

        private static bool IsImplicitReferenceConversion(Type sourceType, Type destinationType)
        {
            return destinationType.IsAssignableFrom(sourceType);
        }

        public static bool IsNonNullableValueType(Type type)
        {
            if (!type.IsValueType)
            {
                return false;
            }
            if (type.IsGenericType)
            {
                return false;
            }
            return (type != typeof(String));
        }

        private static bool IsNullableType(Type type)
        {
            return (type.IsGenericType &&
                    (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        public static bool IsNullableValueType(Type type)
        {
            return (type.IsValueType && IsNullableType(type));
        }

        public static bool ShouldFilterProperty(PropertyDescriptor property, Attribute[] attributes)
        {
            if ((attributes != null) && (attributes.Length != 0))
            {
                foreach (var attribute in attributes)
                {
                    var propertyAttribute = property.Attributes[attribute.GetType()];
                    if (propertyAttribute == null)
                    {
                        if (!attribute.IsDefaultAttribute())
                        {
                            return true;
                        }
                    }
                    else if (!attribute.Match(propertyAttribute))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool TryNumericConversion<T>(object source, out T result)
        {
            Object box;
            var success = TryNumericConversion(source, typeof(T), out box);
            result = (T)box;
            return success;
        }

        private static bool TryNumericConversion(Object source, Type targetType, out Object result)
        {
            TypeCode sourceTypeCode = Type.GetTypeCode(source.GetType());
            TypeCode targetTypeCode = Type.GetTypeCode(targetType);
            switch (sourceTypeCode)
            {
                case TypeCode.Byte:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                result = System.Convert.ChangeType(source, targetTypeCode);
                                return true;
                        }
                        break;
                    }
                case TypeCode.SByte:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                result = System.Convert.ChangeType(source, targetTypeCode);
                                return true;
                        }
                        break;
                    }
                case TypeCode.Int16:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                result = System.Convert.ChangeType(source, targetTypeCode);
                                return true;
                        }
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                result = System.Convert.ChangeType(source, targetTypeCode);
                                return true;
                        }
                        break;
                    }
                case TypeCode.Int32:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                result = System.Convert.ChangeType(source, targetTypeCode);
                                return true;
                        }
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                result = System.Convert.ChangeType(source, targetTypeCode);
                                return true;
                        }
                        break;
                    }
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    {
                        switch (targetTypeCode)
                        {
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                result = System.Convert.ChangeType(source, targetTypeCode);
                                return true;
                        }
                        break;
                    }
                case TypeCode.Single:
                    if (targetTypeCode == TypeCode.Double)
                    {
                        result = System.Convert.ChangeType(source, targetTypeCode);
                        return true;
                    }
                    break;
            }
            result = GetDefaultValueForType(targetType);
            return false;
        }

        public static String GetFriendlyTypeName(Object obj, Boolean includeNamespaces = true)
        {
            var type = (obj as Type) ?? obj.GetType();
            return type.GetFriendlyName(includeNamespaces);
        }
    }
}
