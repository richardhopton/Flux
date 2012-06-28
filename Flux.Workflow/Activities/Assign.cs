using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class Assign<T> : IActivity
    {
        public IInputValue<T> Value { get; set; }
        public IOutputValue<T> To { get; set; }

        public void Execute(IContext context)
        {
            var source = Value.GetValue(context);
            To.SetValue(context, source);
        }
    }
}
