using System;
using System.Linq.Expressions;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class Divide<TLeft, TRight, TResult> : Activity<TResult>
    {
        public IInputValue<TLeft> Left { get; set; }
        public IInputValue<TRight> Right { get; set; }

        public override TResult Execute(IContext context)
        {
            var left = Left.GetValue(context);
            var right = Right.GetValue(context);
            var result = DivideFunction(left, right);
            return result;
        }

        static readonly Func<TLeft, TRight, TResult> DivideFunction;
        static Divide()
        {
            var leftParameter = Expression.Parameter(typeof(TLeft), "left");
            var rightParameter = Expression.Parameter(typeof(TRight), "right");
            DivideFunction = Expression.Lambda<Func<TLeft, TRight, TResult>>(Expression.Divide(leftParameter, rightParameter), leftParameter, rightParameter).Compile();
        }
    }
}
