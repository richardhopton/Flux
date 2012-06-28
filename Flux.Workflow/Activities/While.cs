﻿using System;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Activity")]
    public class While : IActivity
    {
        public IActivity<Boolean> Condition { get; set; }
        public IActivity Activity { get; set; }

        public void Execute(IContext context)
        {
            while (Condition.Execute(context))
            {
                Activity.Execute(context);
            }
        }
    }
}
