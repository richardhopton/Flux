using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class Multiply<TLeft, TRight, TResult> : Activity<TResult>
    {
        public IInputValue<TLeft> Left { get; set; }
        public IInputValue<TRight> Right { get; set; }
        [DefaultValue(false)]
        public Boolean Checked { get; set; }

        public override TResult Execute(IContext context)
        {
            var left = Left.GetValue(context);
            var right = Right.GetValue(context);
            var result = Checked ? MulitplyCheckedFunction(left, right) : MulitplyFunction(left, right);
            return result;
        }

        static readonly Func<TLeft, TRight, TResult> MulitplyFunction;
        static readonly Func<TLeft, TRight, TResult> MulitplyCheckedFunction;
        static Multiply()
        {
            var leftParameter = Expression.Parameter(typeof(TLeft), "left");
            var rightParameter = Expression.Parameter(typeof(TRight), "right");
            MulitplyFunction = Expression.Lambda<Func<TLeft, TRight, TResult>>(Expression.Multiply(leftParameter, rightParameter), leftParameter, rightParameter).Compile();
            MulitplyCheckedFunction = Expression.Lambda<Func<TLeft, TRight, TResult>>(Expression.MultiplyChecked(leftParameter, rightParameter), leftParameter, rightParameter).Compile();
        }
    }
}
