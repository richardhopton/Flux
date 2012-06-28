using System.Collections.Generic;

namespace Flux.Workflow.Interfaces
{
    public interface IVariableScope
    {
        IList<IVariableDefinition> Variables { get; }
    }
}
