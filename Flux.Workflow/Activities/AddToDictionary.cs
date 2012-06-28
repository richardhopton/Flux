using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class AddToDictionary<TKey, TValue> : IActivity
    {
        public IInputValue<TKey> Key { get; set; }
        public IInputValue<TValue> Value { get; set; }
        public IInputValue<IDictionary<TKey, TValue>> Dictionary { get; set; }

        public void Execute(IContext context)
        {
            var dictionary = Dictionary.GetValue(context);
            var key = Key.GetValue(context);
            var value = Value.GetValue(context);
            dictionary[key] = value;
        }
    }
}
