using System;

namespace Flux.Workflow.Interfaces
{
    public interface IContext
    {
        Object GetValue(IValueDefinition definition);
        void SetValue(IValueDefinition reference, Object value);

        IWorkflow Workflow { get; }
    }
}
