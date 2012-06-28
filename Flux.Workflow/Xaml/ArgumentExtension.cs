using System;
using System.Linq;
using System.Xaml;
using Flux.Core;
using Flux.Workflow.Interfaces;

namespace Flux.Workflow.Xaml
{
    public sealed class ArgumentExtension : ValueExtension
    {
        public ArgumentExtension(String name) : base(name) { }

        protected override IValueDefinition GetValueDefinition(IServiceProvider serviceProvider)
        {
            var rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (rootObjectProvider != null)
            {
                var workflow = rootObjectProvider.RootObject as IWorkflow;
                if (workflow != null)
                {
                    foreach (var argumentDefinition in workflow.Arguments
                        .Where(argumentDefinition => argumentDefinition.Name == Name))
                    {
                        return argumentDefinition;
                    }
                }
            }
            throw new XamlParseException(String.Format("Unable to provide argument {0}", Name));
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

            var argumentDefinition = valueDefinition as IArgumentDefinition;
            if (argumentDefinition != null)
            {
                switch (argumentDefinition.Direction)
                {
                    case Direction.In:
                        if (TypeHelper.ContainsCompatibleType(type, typeof(IActivity<>), typeof(IInputValue), typeof(IInputValue<>)))
                        {
                            return typeof(InArgumentReference<>).MakeGenericType(genericParams);
                        }
                        break;
                    case Direction.Out:
                        if (TypeHelper.ContainsCompatibleType(type, typeof(IOutputValue), typeof(IOutputValue<>)))
                        {
                            return typeof(OutArgumentReference<>).MakeGenericType(genericParams);
                        }
                        break;
                    case Direction.InOut:
                        if (TypeHelper.ContainsCompatibleType(type, typeof(IActivity<>), typeof(IInputValue), typeof(IInputValue<>)))
                        {
                            return typeof(InArgumentReference<>).MakeGenericType(genericParams);
                        }
                        if (TypeHelper.ContainsCompatibleType(type, typeof(IOutputValue), typeof(IOutputValue<>)))
                        {
                            return typeof(OutArgumentReference<>).MakeGenericType(genericParams);
                        }
                        break;
                }
            }
            throw new InvalidCastException();
        }
    }
}
