using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    class Subtract<TLeft, TRight, TResult> : Activity<TResult>
    {
        public IInputValue<TLeft> Left { get; set; }
        public IInputValue<TRight> Right { get; set; }
        [DefaultValue(false)]
        public Boolean Checked { get; set; }

        public override TResult Execute(IContext context)
        {
            var left = Left.GetValue(context);
            var right = Right.GetValue(context);
            var result = Checked ? SubtractCheckedFunction(left, right) : SubtractFunction(left, right);
            return result;
        }

        static readonly Func<TLeft, TRight, TResult> SubtractFunction;
        static readonly Func<TLeft, TRight, TResult> SubtractCheckedFunction;
        static Subtract()
        {
            var leftParameter = Expression.Parameter(typeof(TLeft), "left");
            var rightParameter = Expression.Parameter(typeof(TRight), "right");
            SubtractFunction = Expression.Lambda<Func<TLeft, TRight, TResult>>(Expression.Subtract(leftParameter, rightParameter), leftParameter, rightParameter).Compile();
            SubtractCheckedFunction = Expression.Lambda<Func<TLeft, TRight, TResult>>(Expression.SubtractChecked(leftParameter, rightParameter), leftParameter, rightParameter).Compile();
        }
    }
}
