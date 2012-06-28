using System;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class And : Activity<Boolean>
    {
        public IInputValue<Boolean> Left { get; set; }
        public IInputValue<Boolean> Right { get; set; }

        public override Boolean Execute(IContext context)
        {
            var left = Left.GetValue(context);
            var right = Right.GetValue(context);
            var result = left && right;
            return result;
        }
    }
}
