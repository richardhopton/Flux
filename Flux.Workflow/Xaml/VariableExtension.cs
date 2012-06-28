using System;
using System.Linq;
using System.Windows.Markup;
using System.Xaml;
using Flux.Core;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Xaml
{
    [ContentProperty("Name")]
    public sealed class VariableExtension : ValueExtension
    {
        public VariableExtension(String name) : base(name) { }

        protected override IValueDefinition GetValueDefinition(IServiceProvider serviceProvider)
        {
            var xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
            if (xamlSchemaContextProvider != null)
            {
                var ancestorProvider = xamlSchemaContextProvider.SchemaContext as IAncestorProvider;
                if (ancestorProvider != null)
                {
                    foreach (var variableDefinition in ancestorProvider.Ancestors
                        .OfType<IVariableScope>()
                        .SelectMany(scope => scope.Variables
                                                 .Where(variableDefinition => variableDefinition.Name == Name)))
                    {
                        return variableDefinition;
                    }
                }
            }
            throw new XamlParseException(String.Format("Unable to provide variable {0}", Name));
        }

        protected override Type GetNewType(Type targetPropertyType, IValueDefinition valueDefinition)
        {
            var type = targetPropertyType ?? typeof(IInputValue);
            var genericParams = new[] { valueDefinition.Type };
            if (type.IsGenericType)
            {
                genericParams = type.GetGenericArguments();
                type = type.GetGenericTypeDefinition();
            }

            if (TypeHelper.ContainsCompatibleType(type, typeof(IActivity<>), typeof(IInputValue), typeof(IInputValue<>), typeof(IOutputValue), typeof(IOutputValue<>)))
            {
                return typeof(VariableReference<>).MakeGenericType(genericParams);
            }
            throw new InvalidCastException();
        }
    }
}
