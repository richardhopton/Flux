using System;
using System.ComponentModel;
using Flux.Conditions;
using Flux.Workflow.Interfaces;
using Flux.Workflow.Xaml;

namespace Flux.Workflow.Activities
{
    [TypeConverter(typeof(CompareConverter))]
    public class Compare : Activity<Boolean>
    {
        public IInputValue Operand { get; set; }
        public Comparison Comparison { get; set; }
        public IInputValue Value { get; set; }

        public Compare() { }
        public Compare(IInputValue operand, Comparison comparision, IInputValue value)
        {
            Operand = operand;
            Comparison = comparision;
            Value = value;
        }

        public override Boolean Execute(IContext context)
        {
            var operand = Operand.GetValue(context);
            var value = Value.GetValue(context);

            var comparisonCondition = new ComparisonCondition
            {
                Operand = operand,
                Comparison = Comparison,
                Value = value
            };

            return comparisonCondition.Result;

        }
    }
}
