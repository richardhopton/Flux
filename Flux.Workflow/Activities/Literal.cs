using System.ComponentModel;
using Flux.Workflow.Interfaces;
using Flux.Workflow.Xaml;

namespace Flux.Workflow.Activities
{
    [TypeConverter(typeof(LiteralConverter))]
    public class Literal<T> : Activity<T>
    {
        private readonly T _value;
        public Literal(T value)
        {
            _value = value;
        }

        public T Value
        {
            get { return _value; }
        }

        public override T Execute(IContext context)
        {
            return _value;
        }
    }
}
