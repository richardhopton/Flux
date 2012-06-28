using System.ComponentModel;
using Flux.Core;
using Flux.Workflow.Interfaces;
using Flux.Workflow.Xaml;

namespace Flux.Workflow
{
    [TypeConverter(typeof(ValueReferenceConverter))]
    internal abstract class ValueReference
    {
        private readonly IValueDefinition _valueDefinition;

        protected ValueReference(IValueDefinition valueDefinition)
        {
            _valueDefinition = valueDefinition;
        }

        public IValueDefinition ValueDefinition
        {
            get { return _valueDefinition; }
        }
    }

    internal sealed class VariableReference<T> : ValueReference, IActivity<T>, IInputValue<T>, IOutputValue<T>
    {
        public VariableReference(IValueDefinition valueDefinition) : base(valueDefinition) { }        

        T IActivity<T>.Execute(IContext context)
        {
            return GetValue(context);
        }

        void IActivity.Execute(IContext context) { }

        public T GetValue(IContext context)
        {
            var value = context.GetValue(ValueDefinition);
            return TypeHelper.Convert<T>(value);
        }

        object IInputValue.GetValue(IContext context)
        {
            return GetValue(context);
        }

        public void SetValue(IContext context, T value)
        {
            context.SetValue(ValueDefinition, value);
        }

        void IOutputValue.SetValue(IContext context, object value)
        {
            SetValue(context, TypeHelper.Convert<T>(value));
        }
    }

    internal sealed class InArgumentReference<T> : ValueReference, IActivity<T>, IInputValue<T>
    {
        public InArgumentReference(IValueDefinition valueDefinition) : base(valueDefinition) { }        

        T IActivity<T>.Execute(IContext context)
        {
            return GetValue(context);
        }

        void IActivity.Execute(IContext context) { }

        public T GetValue(IContext context)
        {
            var value = context.GetValue(ValueDefinition);
            return TypeHelper.Convert<T>(value);
        }

        object IInputValue.GetValue(IContext context)
        {
            return GetValue(context);
        }
    }

    internal sealed class OutArgumentReference<T> : ValueReference, IOutputValue<T>
    {
        public OutArgumentReference(IValueDefinition valueDefinition) : base(valueDefinition) { }        

        public void SetValue(IContext context, T value)
        {
            context.SetValue(ValueDefinition, value);
        }

        void IOutputValue.SetValue(IContext context, object value)
        {
            SetValue(context, TypeHelper.Convert<T>(value));
        }
    }
}