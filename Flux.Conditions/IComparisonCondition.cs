using System;

namespace Flux.Conditions
{
    public interface IComparisonCondition : ICondition
    {
        Object Operand { get; set; }
        Object Value { get; set; }
        Comparison Comparison { get; set; }
    }
}
