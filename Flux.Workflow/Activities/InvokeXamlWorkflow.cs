using System;
using System.Collections.Generic;
using Flux.Workflow.Interfaces;
using Flux.Workflow.Xaml;

namespace Flux.Workflow.Activities
{
    public class InputArgument
    {
        public String Name { get; set; }
        public IInputValue Value { get; set; }

        public Object GetValue(IContext context)
        {
            return Value.GetValue(context);
        }
    }

    public class OutputArgument
    {
        public String Name { get; set; }
        public IOutputValue Value { get; set; }

        public void SetValue(IContext context, Object value)
        {
            Value.SetValue(context, value);
        }
    }

    public class InvokeXamlWorkflow : IActivity
    {
        public List<InputArgument> InputArguments { get; private set; }
        public List<OutputArgument> OutputArguments { get; private set; }
        public String FileName { get; set; }

        public InvokeXamlWorkflow()
        {
            InputArguments = new List<InputArgument>();
            OutputArguments = new List<OutputArgument>();
        }

        public void Execute(IContext context)
        {
            var workflow = XamlWorkflow.Load(FileName);
            var inputs = new Dictionary<String, Object>();
            foreach (var inputArgument in InputArguments)
            {
                inputs[inputArgument.Name] = inputArgument.Value.GetValue(context);
            }
            var outputs = WorkflowInvoker.Execute(workflow, inputs);
            foreach (var outputArgument in OutputArguments)
            {
                outputArgument.Value.SetValue(context, outputs.GetValueOrDefault(outputArgument.Name));
            }
        }
    }
}
