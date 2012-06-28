using System;
using System.ComponentModel;
using Flux.Core;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class GetPropertyValue<T, TResult> : Activity<TResult>
    {
        public IInputValue<T> Object { get; set; }
        public String PropertyName { get; set; }

        [DefaultValue(null)]
        public IInputValue Index { get; set; }

        public override TResult Execute(IContext context)
        {
            var obj = Object.GetValue(context);
            var index = default(Object[]);
            if (Index != null)
            {
                var indexValue = Index.GetValue(context);
                if (indexValue != null)
                {
                    index = indexValue as Object[] ?? new[] { indexValue };
                }
            }
            var result = obj.GetType().GetProperty(PropertyName).GetValue(obj, index);
            return TypeHelper.Convert<TResult>(result);
        }
    }
}
