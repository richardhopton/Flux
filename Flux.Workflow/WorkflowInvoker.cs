using System;
using System.Collections.Generic;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow
{
    public static class WorkflowInvoker
    {
        public static IDictionary<String, Object> Execute(IWorkflow workflow, IDictionary<String, Object> inputs)
        {
            using (var context = new Context(workflow))
            {
                foreach (var argument in workflow.Arguments)
                {
                    if (((argument.Direction == Direction.In) ||
                         (argument.Direction == Direction.InOut)) &&
                        inputs.ContainsKey(argument.Name))
                    {
                        context.SetValue(argument, inputs[argument.Name]);
                    }
                }
                workflow.Execute(context);
                var outputs = new Dictionary<String, Object>();
                foreach (var argument in workflow.Arguments)
                {
                    if ((argument.Direction == Direction.Out) ||
                        (argument.Direction == Direction.InOut))
                    {
                        outputs[argument.Name] = context.GetValue(argument);
                    }
                }
                return outputs;
            }
        }
    }
}
