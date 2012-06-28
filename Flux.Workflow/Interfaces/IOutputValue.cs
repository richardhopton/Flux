using System;

namespace Flux.Workflow.Interfaces
{
    public interface IOutputValue
    {
        void SetValue(IContext context, Object value);
    }

    public interface IOutputValue<in T> : IOutputValue
    {
        void SetValue(IContext context, T value);
    }
}
