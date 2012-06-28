using System;
using System.Threading;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    class Delay : IActivity
    {
        public IInputValue<Int32> Milliseconds { get; set; }

        public void Execute(IContext context)
        {
            var milliseconds = Milliseconds.GetValue(context);
            Thread.Sleep(milliseconds);
        }
    }
}
