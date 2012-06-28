using System;
using System.ComponentModel;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow
{
    public class ValueDefinition : IValueDefinition
    {
        public String Name { get; set; }

        [DefaultValue(typeof(Object))]
        public Type Type { get; set; }

        public ValueDefinition()
        {
            Type = typeof(Object);
        }
    }
}
