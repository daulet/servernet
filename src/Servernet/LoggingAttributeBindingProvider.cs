using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Servernet
{
    internal class LoggingAttributeBindingProvider : IBindingProvider
    {
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Determine whether we should bind to the current parameterInfo
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<LoggingAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            if (parameter.ParameterType != typeof(Logger))
            {
                throw new InvalidOperationException(
                    $"Can't bind SampleAttribute to type '{parameter.ParameterType}'.");
            }

            return Task.FromResult<IBinding>(new LoggingBinding(parameter));
        }

        private class LoggingBinding : IBinding
        {
            private readonly ParameterInfo _parameterInfo;

            public LoggingBinding(ParameterInfo parameterInfo)
            {
                _parameterInfo = parameterInfo;
            }

            public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
            {
                return Task.FromResult<IValueProvider>(new LoggingValueBinder(_parameterInfo));
            }

            public Task<IValueProvider> BindAsync(BindingContext context)
            {
                return Task.FromResult<IValueProvider>(new LoggingValueBinder(_parameterInfo));
            }

            public ParameterDescriptor ToParameterDescriptor()
            {
                return new ParameterDescriptor
                {
                    Name = _parameterInfo.Name,
                    DisplayHints = new ParameterDisplayHints
                    {
                        Description = "Logger",
                        DefaultValue = "null",
                        Prompt = "Enter a Logger reference"
                    }
                };
            }

            public bool FromAttribute => true;

            private class LoggingValueBinder : IValueBinder
            {
                private readonly ParameterInfo _parameterInfo;
                private object _value;

                public LoggingValueBinder(ParameterInfo parameterInfo)
                {
                    _parameterInfo = parameterInfo;
                }

                public object GetValue()
                {
                    return _value;
                }

                public string ToInvokeString()
                {
                    return "Logger";
                }

                public Type Type => _parameterInfo.ParameterType;

                public Task SetValueAsync(object value, CancellationToken cancellationToken)
                {
                    _value = value;

                    return Task.FromResult(true);
                }
            }
        }
    }
}
