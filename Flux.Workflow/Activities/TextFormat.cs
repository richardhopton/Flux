using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Items")]
    public class TextFormat : Activity<String>
    {
        public TextFormat()
        {
            Items = new List<IInputValue>();
        }

        public IInputValue<String> Format { get; set; }
        public IList<IInputValue> Items { get; set; }

        public override String Execute(IContext context)
        {
            var items = Items.Select(i => i.GetValue(context)).ToArray();
            var format = Format.GetValue(context);
            return String.Format(format, items);
        }
    }
}
