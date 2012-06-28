using System.Collections.Generic;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow
{
    [ContentProperty("Activity")]
    public class Workflow : IWorkflow
    {
        public List<IArgumentDefinition> Arguments { get; private set; }
        public IActivity Activity { get; set; }

        public Workflow()
        {
            Arguments = new List<IArgumentDefinition>();
        }

        public void Execute(IContext context)
        {
            Activity.Execute(context);
        }

        IList<IArgumentDefinition> IWorkflow.Arguments
        {
            get { return Arguments; }
        }
    }
}
