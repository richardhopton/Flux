using System;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow
{
    public abstract class Activity<T> : IActivity<T>, IInputValue<T>
    {
        public abstract T Execute(IContext context);

        void IActivity.Execute(IContext context)
        {
            Execute(context);
        }

        T IInputValue<T>.GetValue(IContext context)
        {
            return Execute(context);
        }

        Object IInputValue.GetValue(IContext context)
        {
            return Execute(context);
        }
    }
}
