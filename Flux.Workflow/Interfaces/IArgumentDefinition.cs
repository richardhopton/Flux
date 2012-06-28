namespace Flux.Workflow.Interfaces
{
    public interface IArgumentDefinition : IValueDefinition
    {
        Direction Direction { get; set;  }
    }
}
