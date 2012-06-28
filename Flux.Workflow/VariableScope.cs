using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow
{
    public abstract class VariableScope : IVariableScope
    {
        public List<IVariableDefinition> Variables { get; private set; }

        protected VariableScope()
        {
            Variables = new List<IVariableDefinition>();
        }

        IList<IVariableDefinition> IVariableScope.Variables
        {
            get { return Variables; }
        }
    }
}