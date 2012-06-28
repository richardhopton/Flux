using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class Add<TLeft, TRight, TResult> : Activity<TResult>
    {
        public IInputValue<TLeft> Left { get; set; }
        public IInputValue<TRight> Right { get; set; }
        [DefaultValue(false)]
        public Boolean Checked { get; set; }

        public override TResult Execute(IContext context)
        {
            var left = Left.GetValue(context);
            var right = Right.GetValue(context);
            var result = Checked ? AddCheckedFunction(left, right) : AddFunction(left, right);
            return result;
        }

        static readonly Func<TLeft, TRight, TResult> AddFunction;
        static readonly Func<TLeft, TRight, TResult> AddCheckedFunction;
        static Add()
        {
            var leftParameter = Expression.Parameter(typeof(TLeft), "left");
            var rightParameter = Expression.Parameter(typeof(TRight), "right");
            AddFunction = Expression.Lambda<Func<TLeft, TRight, TResult>>(Expression.Add(leftParameter, rightParameter), leftParameter, rightParameter).Compile();
            AddCheckedFunction = Expression.Lambda<Func<TLeft, TRight, TResult>>(Expression.AddChecked(leftParameter, rightParameter), leftParameter, rightParameter).Compile();
        }
    }
}
