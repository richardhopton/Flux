using System.Collections;
using System.Collections.Generic;

namespace Flux.Conditions
{
    public interface IConditionGroup : IList<ICondition>, ICondition
    {
        LogicalOperator LogicalOperator { get; set; }
    }
}
