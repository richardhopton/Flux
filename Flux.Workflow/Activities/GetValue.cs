using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class GetValue<T> : Activity<T>
    {
        public IInputValue<T> Expression { get; set; }

        public override T Execute(IContext context)
        {
            return Expression.GetValue(context);
        }
    }
}
