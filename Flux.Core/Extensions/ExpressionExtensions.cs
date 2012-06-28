using System.Reflection;

// ReSharper disable CheckNamespace
namespace System.Linq.Expressions
// ReSharper restore CheckNamespace
{
    public static class ExpressionExtensions
    {
        public static MemberInfo GetMemberInfo<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            return GetMemberInfo(lambdaExpression);
        }

        public static String GetName<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            var memberInfo = expression.GetMemberInfo();
            return memberInfo != null ? memberInfo.Name : null;
        }


        public static MemberInfo GetMemberInfo<T>(this Expression<Func<T, Object>> expression)
        {
            return expression.GetMemberInfo<T, Object>();
        }

        public static String GetName<T>(this Expression<Func<T, Object>> expression)
        {
            return expression.GetName<T, Object>();
        }

        public static MemberInfo GetMemberInfo<T>(this Expression<Action<T>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            return GetMemberInfo(lambdaExpression);
        }

        public static String GetName<T>(this Expression<Action<T>> expression)
        {
            var memberInfo = expression.GetMemberInfo();
            return memberInfo != null ? memberInfo.Name : null;
        }

        public static MemberInfo GetMemberInfo<TResult>(this Expression<Func<TResult>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            return GetMemberInfo(lambdaExpression);
        }

        public static String GetName<TResult>(this Expression<Func<TResult>> expression)
        {
            var memberInfo = expression.GetMemberInfo();
            return memberInfo != null ? memberInfo.Name : null;
        }

        public static MemberInfo GetMemberInfo(this Expression<Func<Object>> expression)
        {
            return expression.GetMemberInfo<Object>();
        }

        public static String GetName(this Expression<Func<Object>> expression)
        {
            return expression.GetName<Object>();
        }

        public static MemberInfo GetMemberInfo(this Expression<Action> expression)
        {
            var lambdaExpression = expression as LambdaExpression;
            return GetMemberInfo(lambdaExpression);
        }

        public static String GetName(this Expression<Action> expression)
        {
            var memberInfo = expression.GetMemberInfo();
            return memberInfo != null ? memberInfo.Name : null;
        }

        private static MemberInfo GetMemberInfo(LambdaExpression lambdaExpression)
        {
            var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return methodCallExpression.Method;
            }

            MemberExpression memberExpression;
            var unaryExpression = lambdaExpression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambdaExpression.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                return memberExpression.Member;
            }
            return null;
        }


        public static MemberInfo GetMemberInfo<T>(this Action<T> action)
        {
            return action.Method;
        }

        public static String GetName<T>(this Action<T> action)
        {
            return action.Method.Name;
        }

        public static MemberInfo GetMemberInfo(this Action action)
        {
            return action.Method;
        }

        public static String GetName(this Action action)
        {
            return action.Method.Name;
        }
    }
}