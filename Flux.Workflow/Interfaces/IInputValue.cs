using System;

namespace Flux.Workflow.Interfaces
{
    public interface IInputValue
    {
        Object GetValue(IContext context);
    }

    public interface IInputValue<out T> : IInputValue
    {
        new T GetValue(IContext context);
    }

}
