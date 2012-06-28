using System.Collections.Generic;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Activities")]
    public class Sequence : VariableScope, IActivity
    {
        public IList<IActivity> Activities { get; private set; }

        public Sequence()
        {
            Activities = new List<IActivity>();
        }

        public void Execute(IContext context)
        {
            using (var sequenceContext = new Context(context, this))
            {
                foreach (var activity in Activities)
                {
                    activity.Execute(sequenceContext);
                }
            }
        }
    }
}
