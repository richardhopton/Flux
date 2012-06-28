using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace Flux.Workflow.Xaml
{
    public class ValueReferenceConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof(MarkupExtension)) ||
                   base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }

            var valueReference = value as ValueReference;
            if (valueReference == null)
            {
                throw new Exception("Wrong Type");
            }

            if (destinationType == typeof(MarkupExtension))
            {
                var valueDefinition = valueReference.ValueDefinition;
                if (valueDefinition is ArgumentDefinition)
                {
                    return new ArgumentExtension(valueDefinition.Name);
                }
                if (valueDefinition is CurrentItemDefinition)
                {
                    return new CurrentItemExtension();
                }
                if (valueDefinition is VariableDefinition)
                {
                    return new VariableExtension(valueDefinition.Name);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
