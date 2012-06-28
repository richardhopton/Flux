using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class ClearDictionary<TKey, TValue> : IActivity
    {
        public IInputValue<IDictionary<TKey, TValue>> Dictionary { get; set; }

        public void Execute(IContext context)
        {
            var dictionary = Dictionary.GetValue(context);
            dictionary.Clear();
        }
    }
}
