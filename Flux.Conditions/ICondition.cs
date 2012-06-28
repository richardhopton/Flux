using System;

namespace Flux.Conditions
{
    public interface ICondition
    {
        NegationOperator NegationOperator { get; set; }
        Boolean Result { get; }
        event EventHandler OnResultChanged;
    }
}
