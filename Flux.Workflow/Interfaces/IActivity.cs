
namespace Flux.Workflow.Interfaces
{
    public interface IActivity
    {
        void Execute(IContext context);
    }

    public interface IActivity<out T> : IActivity
    {
        new T Execute(IContext context);
    }
}
