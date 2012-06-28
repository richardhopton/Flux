using System;
using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class RemoveFromCollection<T> : Activity<Boolean>
    {
        public IInputValue<T> Item { get; set; }
        public IInputValue<ICollection<T>> Collection { get; set; }

        public override Boolean Execute(IContext context)
        {
            var collection = Collection.GetValue(context);
            var item = Item.GetValue(context);
            return collection.Remove(item);
        }
    }
}
