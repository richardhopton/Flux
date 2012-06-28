using System;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Activities
{
    [ContentProperty("Text")]
    public class ShowMessageActivity : IActivity
    {
        public IInputValue<String> Text { get; set; }

        public void Execute(IContext context)
        {
            var text = Text.GetValue(context);
            Console.WriteLine(text);
        }
    }
}
