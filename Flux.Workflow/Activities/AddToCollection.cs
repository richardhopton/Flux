using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class AddToCollection<T> : IActivity
    {
        public IInputValue<T> Item { get; set; }
        public IInputValue<ICollection<T>> Collection { get; set; }

        public void Execute(IContext context)
        {
            var collection = Collection.GetValue(context);
            var item = Item.GetValue(context);
            collection.Add(item);
        }
    }
}
