using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Activity")]
    public class ForEach<T> : IActivity, IEnumerableVariableScope
    {
        public IInputValue<IEnumerable<T>> Items { get; set; }
        public IActivity Activity { get; set; }

        [DefaultValue(false)]
        public Boolean Parallel { get; set; }

        public void Execute(IContext context)
        {
            var items = Items.GetValue(context);
            if (Parallel)
            {
                items = items.AsParallel();
            }
            foreach (var item in items)
            {
                using (var forEachContext = new Context(context, this))
                {
                    forEachContext.SetValue(_currentItemVariable, item);
                    Activity.Execute(forEachContext);
                }
            }
        }

        private readonly IVariableDefinition _currentItemVariable = new CurrentItemDefinition { Name = "CurrentItem", Type = typeof(T) };

        IVariableDefinition IEnumerableVariableScope.CurrentItemVariable
        {
            get { return _currentItemVariable; }
        }
    }
}
