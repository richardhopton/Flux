using System;
using System.ComponentModel;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class SetPropertyValue<T, TValue> : IActivity
    {
        public IInputValue<T> Object { get; set; }
        public IInputValue<String> PropertyName { get; set; }
        public IInputValue<TValue> Value { get; set; }

        [DefaultValue(null)]
        public IInputValue Index { get; set; }

        public void Execute(IContext context)
        {
            var obj = Object.GetValue(context);
            var propertyName = PropertyName.GetValue(context);
            var index = default(Object[]);
            if (Index != null)
            {
                var indexValue = Index.GetValue(context);
                if (indexValue != null)
                {
                    index = indexValue as Object[] ?? new[] { indexValue };
                }
            }
            var value = Value.GetValue(context);
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, index);
        }
    }
}
