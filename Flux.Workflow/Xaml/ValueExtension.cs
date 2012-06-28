using System;
using System.Reflection;
using System.Windows.Markup;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Xaml
{
    public abstract class ValueExtension : MarkupExtension
    {
        [ConstructorArgument("name")]
        public String Name { get; set; }

        protected ValueExtension(String name)
        {
            Name = name;
        }

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

        protected abstract IValueDefinition GetValueDefinition(IServiceProvider serviceProvider);
        protected abstract Type GetNewType(Type targetPropertyType, IValueDefinition valueDefinition);
    }
}