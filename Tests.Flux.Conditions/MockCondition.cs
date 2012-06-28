using System;
using Flux.Conditions;

namespace Tests.Flux.Conditions
{
    internal class MockCondition : Condition
    {
        public MockCondition(Boolean result)
        {
            SetResult(result);
        }

        public new void SetResult(Boolean result)
        {
            base.SetResult(result);
        }

        public override string ToString()
        {
            return String.Format("of {0}", Result);
        }
    }
}
