using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using Flux.Core;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Xaml
{
    public sealed class CurrentItemExtension : MarkupExtension
    {
        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueDefinition = GetValueDefinition(serviceProvider);
            var targetPropertyType = GetTargetPropertyType(serviceProvider);
            var newType = GetNewType(targetPropertyType, valueDefinition);
            return Activator.CreateInstance(newType, valueDefinition);
        }

        private Type GetTargetPropertyType(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
                return null;
            var targetProperty = provideValueTarget.TargetProperty as PropertyInfo;
            return targetProperty == null ? null : targetProperty.PropertyType;
        }

        private IValueDefinition GetValueDefinition(IServiceProvider serviceProvider)
        {
            var xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
            if (xamlSchemaContextProvider != null)
            {
                var ancestorProvider = xamlSchemaContextProvider.SchemaContext as IAncestorProvider;
                if (ancestorProvider != null)
                {
                    foreach (var enumerableScope in ancestorProvider.Ancestors
                        .OfType<IEnumerableVariableScope>())
                    {
                        return enumerableScope.CurrentItemVariable;
                    }
                }
            }
            throw new XamlParseException("Unable to provide current item");
        }

        private Type GetNewType(Type targetPropertyType, IValueDefinition valueDefinition)
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
