using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Values")]
    public class TextConcat : Activity<String>
    {
        public IList<IInputValue> Values { get; private set; }
        public TextConcat()
        {
            Values = new List<IInputValue>();
        }

        public override String Execute(IContext context)
        {
            var results = Values
                .Select(v => v.GetValue(context))
                .ToArray();
            return String.Concat(results);
        }
    }
}
