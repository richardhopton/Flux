using System;
using System.ComponentModel;
using System.Windows.Markup;
using Flux.Conditions;
using Flux.Workflow.Activities;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Xaml
{
    public class CompareExtension : MarkupExtension
    {
        [ConstructorArgument("operand")]
        public IInputValue Operand { get; set; }

        [ConstructorArgument("comparison")]
        public Comparison Comparison { get; set; }

        [ConstructorArgument("value")]
        [DefaultValue(null)]
        public IInputValue Value { get; set; }

        public CompareExtension() { }
        public CompareExtension(IInputValue operand, Comparison comparison, IInputValue value)
        {
            Operand = operand;
            Value = value;
            Comparison = comparison;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new Compare(Operand, Comparison, Value);
        }
    }
}
