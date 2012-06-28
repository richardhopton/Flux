using System;
using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class ExistsInDictionary<TKey, TValue> : Activity<Boolean>
    {
        public IInputValue<TKey> Key { get; set; }
        public IInputValue<IDictionary<TKey, TValue>> Dictionary { get; set; }

        public override Boolean Execute(IContext context)
        {
            var dictionary = Dictionary.GetValue(context);
            var key = Key.GetValue(context);
            return dictionary.ContainsKey(key);
        }
    }
}
