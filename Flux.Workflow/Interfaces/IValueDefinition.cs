using System;

namespace Flux.Workflow.Interfaces
{
    public interface IValueDefinition
    {
        String Name { get; }
        Type Type { get; }
    }
}
