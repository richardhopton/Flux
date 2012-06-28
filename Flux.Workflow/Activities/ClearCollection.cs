using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class ClearCollection<T> : IActivity
    {
        public IInputValue<ICollection<T>> Collection { get; set; }

        public void Execute(IContext context)
        {
            var collection = Collection.GetValue(context);
            collection.Clear();
        }
    }
}
