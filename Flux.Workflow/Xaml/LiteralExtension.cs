using System;
using System.Reflection;
using System.Windows.Markup;
using Flux.Workflow.Activities;

namespace Flux.Workflow.Xaml
{
    public class LiteralExtension : MarkupExtension
    {
        [ConstructorArgument("value")]
        public String Value { get; set; }

        public LiteralExtension(String value)
        {
            Value = value;
        }

        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetPropertyType = GetTargetPropertyType(serviceProvider);
            var newType = typeof(Literal<Object>);
            var newValue = Value as Object;
            if ((targetPropertyType != null) &&
                targetPropertyType.IsGenericType)
            {
                var typeParam = targetPropertyType.GetGenericArguments()[0];
                newType = typeof(Literal<>).MakeGenericType(typeParam);
                newValue = Convert.ChangeType(Value, typeParam);
            }

            return Activator.CreateInstance(newType, newValue);
        }

        private Type GetTargetPropertyType(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
                return null;
            
            var targetProperty = provideValueTarget.TargetProperty as PropertyInfo;
            return targetProperty == null ? null : targetProperty.PropertyType;
        }

    }
}
