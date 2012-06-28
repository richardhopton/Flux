using System;
using System.ComponentModel;
using System.Windows.Markup;
using Flux.Workflow.Activities;

namespace Flux.Workflow.Xaml
{
    public class CompareConverter : TypeConverter
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

            var compare = value as Compare;
            if (compare == null)
            {
                throw new Exception("Wrong Type");
            }

            if (destinationType == typeof(MarkupExtension))
            {
                return new CompareExtension(compare.Operand, compare.Comparison, compare.Value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
