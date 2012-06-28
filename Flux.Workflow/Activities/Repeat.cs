using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Activity")]
    public class Repeat : IActivity
    {
        public IInputValue<Int32> Count { get; set; }
        public IActivity Activity { get; set; }

        public void Execute(IContext context)
        {
            var count = Count.GetValue(context);
            for (var index = 0; index < count; index++)
            {
                Activity.Execute(context);
            }
        }
    }
}
