using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Cases")]
    public class Switch<T> : IActivity
    {
        public IInputValue<T> Expression { get; set; }
        public Dictionary<T, IActivity> Cases { get; private set; }

        [DefaultValue(null)]
        public IActivity Default { get; set; }

        public Switch()
        {
            Cases = new Dictionary<T, IActivity>();
        }

        public void Execute(IContext context)
        {
            var key = Expression.GetValue(context);
            IActivity activity;
            if (!Cases.TryGetValue(key, out activity))
            {
                activity = Default;
            }
            if (activity != null)
            {
                activity.Execute(context);
            }
        }
    }
}
