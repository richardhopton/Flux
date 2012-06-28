using System;
using System.Collections.Generic;
using Flux.Core;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow
{
    public class Context : VariableScope, IContext, IDisposable
    {
        private class ValueStore
        {
            public Object Value { get; set; }
        }

        private readonly Dictionary<IValueDefinition, ValueStore> _values = new Dictionary<IValueDefinition, ValueStore>();

        public IWorkflow Workflow { get; private set; }

        public Context(IWorkflow workflow)
        {
            Workflow = workflow;
            foreach (var argument in workflow.Arguments)
            {
                _values[argument] = new ValueStore();
            }
        }

        public Context(IWorkflow workflow, IVariableScope variableScope)
            : this(workflow)
        {
            if (variableScope == null)
                return;
            
            foreach (var variable in variableScope.Variables)
            {
                Variables.Add(variable);
                _values[variable] = new ValueStore();
            }
        }

        public Context(IContext context, IVariableScope variableScope)
            : this(context.Workflow, variableScope)
        {
            var contextVariableScope = context as Context;
            if (contextVariableScope == null)
                return;
            
            foreach (var argument in Workflow.Arguments)
            {
                _values[argument] = contextVariableScope._values[argument];
            }

            foreach (var variable in contextVariableScope.Variables)
            {
                Variables.Add(variable);
                _values[variable] = contextVariableScope._values[variable];
            }
        }

        public Context(IContext context, IEnumerableVariableScope enumerableVariableScope)
            : this(context, default(IVariableScope))
        {
            var variable = enumerableVariableScope.CurrentItemVariable;
            Variables.Add(variable);
            _values[variable] = new ValueStore();
        }

        private void CheckValueDefinitionRegistered(IValueDefinition valueDefinition)
        {
            var argumentDefinition = valueDefinition as IArgumentDefinition;
            if (argumentDefinition != null)
            {
                if (_values.ContainsKey(argumentDefinition))
                {
                    return;
                }
                throw new InvalidOperationException("Argument not registered");
            }
            var variableDefinition = valueDefinition as IVariableDefinition;
            if (variableDefinition != null)
            {
                if (_values.ContainsKey(variableDefinition))
                {
                    return;
                }
                throw new InvalidOperationException("Variable not registered");
            }
            throw new ArgumentException("Value definition could not be found");
        }

        public Object GetValue(IValueDefinition valueDefinition)
        {
            CheckValueDefinitionRegistered(valueDefinition);
            return _values[valueDefinition].Value;
        }

        public void SetValue(IValueDefinition valueDefinition, Object value)
        {
            CheckValueDefinitionRegistered(valueDefinition);
            _values[valueDefinition].Value = TypeHelper.Convert(value, valueDefinition.Type);
        }

        public void Dispose()
        {
            _values.Clear();
            Workflow = null;
            GC.SuppressFinalize(this);
        }
    }
}