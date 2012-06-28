using System;
using System.ComponentModel;
using System.Windows.Markup;
using Flux.Workflow.Activities;

namespace Flux.Workflow.Xaml
{
    public class LiteralConverter : TypeConverter
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

            var type = value.GetType();
            if (!type.IsGenericType &&
                (type.GetGenericTypeDefinition() != typeof(Literal<>)))
            {
                throw new Exception("Wrong Type");
            }

            if (destinationType == typeof(MarkupExtension))
            {
                var literalValue = type.GetProperty("Value").GetValue(value, null) ?? String.Empty;
                return new LiteralExtension(literalValue.ToString());
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
