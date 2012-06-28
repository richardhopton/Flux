using System;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    public class If : IActivity
    {
        public IActivity<Boolean> Condition { get; set; }
        public IActivity Then { get; set; }
        public IActivity Else { get; set; }

        public void Execute(IContext context)
        {
            var conditionResult = Condition.Execute(context);
            if (conditionResult)
            {
                Then.Execute(context);
            }
            else if (Else != null)
            {
                Else.Execute(context);
            }
        }
    }
}
