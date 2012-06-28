using System.Collections.Generic;

namespace Flux.Workflow.Interfaces
{
    public interface IWorkflow
    {
        IList<IArgumentDefinition> Arguments { get; }
        IActivity Activity { get; set; }
        void Execute(IContext context);
    }
}
