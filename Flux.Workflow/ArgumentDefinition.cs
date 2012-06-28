using Flux.Workflow.Interfaces;

namespace Flux.Workflow
{
    public class ArgumentDefinition : ValueDefinition, IArgumentDefinition
    {
        public Direction Direction { get; set; }
    }
}
