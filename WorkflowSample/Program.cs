using System;
using System.Collections.Generic;
using Flux.Workflow;
using Flux.Workflow.Xaml;

namespace WorkflowSample
{
    class Program
    {

        static void Main(string[] args)
        {
            var workflow = XamlWorkflow.Load("Workflow.xaml");
            
            WorkflowInvoker.Execute(workflow, new Dictionary<String, Object> { { "Message", "Hello" }, { "Items", new[] { "One", "Two", "Three" } } });
            Console.WriteLine();
            WorkflowInvoker.Execute(workflow, new Dictionary<String, Object> { { "Message", "Goodbye" }, { "Items", new String[] { } } });
            Console.ReadLine();
        }
    }
}
