using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet.Generator.Definition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Servernet.Generator
{
    internal class FunctionBuilder
    {
        private readonly Function _function;
        private readonly TypeSwitch<MethodInfo> _methodAttributeSwitch;
        private readonly TypeSwitch<ParameterInfo> _parameterAttributeSwitch;

        public FunctionBuilder(
            Type functionType,
            MethodInfo functionMethod)
        {
            _function = new Function()
            {
                Bindings = new List<IBinding>(),
                Disabled = false,
                Name = functionType.Name,
                EntryPoint = $"{functionType.FullName}.{functionMethod.Name}",
                ScriptFile = $"{Path.GetFileName(functionType.Assembly.Location)}",
            };

            var attribute = functionMethod.GetCustomAttribute<FunctionNameAttribute>(inherit: false);
            if (attribute != null)
            {
                _function.Name = attribute.Name;
            }

            _methodAttributeSwitch = new TypeSwitch<MethodInfo>()
                .Case((MethodInfo method, Temp.HttpOutputAttribute x) => { _function.Bindings.Add(new HttpOutputBinding("$return")); });

            _parameterAttributeSwitch = new TypeSwitch<ParameterInfo>()
                .Case((ParameterInfo parameter, BlobAttribute x) =>
                {
                    // based on https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob
                    var binding = parameter.IsOut
                        ? (IBinding) new BlobOutputBinding(functionType, parameter.Name, x)
                        : (IBinding) new BlobInputBinding(functionType, parameter.Name, x);
                    _function.Bindings.Add(binding);
                })
                .Case((ParameterInfo parameter, BlobTriggerAttribute x) => { _function.Bindings.Add(new BlobTriggerBinding(functionType, parameter.Name, x)); })
                .Case((ParameterInfo parameter, HttpTriggerAttribute x) =>
                {
                    x.Route = x.Route?? _function.Name;
                    if (string.IsNullOrEmpty(x.WebHookType))
                    {
                        _function.Bindings.Add(new HttpTriggerBinding(parameter.Name, x));
                    }
                    else
                    {
                        _function.Bindings.Add(new WebHookTriggerBinding(parameter.Name, x));
                    }
                })
                .Case((ParameterInfo parameter, QueueAttribute x) => { _function.Bindings.Add(new QueueOutputBinding(functionType, parameter.Name, x)); })
                .Case((ParameterInfo parameter, QueueTriggerAttribute x) => { _function.Bindings.Add(new QueueTriggerBinding(functionType, parameter.Name, x)); })
                .Case((ParameterInfo parameter, SecretAttribute x) => { _function.Bindings.Add(new BlobInputBinding(functionType, parameter.Name, x)); })
                .Case((ParameterInfo parameter, TableAttribute x) =>
                {
                    // based on https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table
                    IBinding binding;
                    if (parameter.IsOut)
                    {
                        binding = new TableOutputBinding(functionType, parameter.Name, x);
                    }
                    else
                    {
                        if (parameter.ParameterType.IsAssignableFrom(typeof(ICollector<object>)) ||
                            parameter.ParameterType.IsAssignableFrom(typeof(IAsyncCollector<object>)) ||
                            parameter.ParameterType.IsAssignableFrom(typeof(CloudTable)))
                        {
                            binding = new TableOutputBinding(functionType, parameter.Name, x);
                        }
                        else
                        {
                            binding = new TableInputBinding(functionType, parameter.Name, x);
                        }
                    }
                    _function.Bindings.Add(binding);
                })
                .Case((ParameterInfo parameter, TimerTriggerAttribute x) => { _function.Bindings.Add(new TimerTriggerBinding(parameter.Name, x)); })
                ;
        }

        public void AddBinding(MethodInfo method, object attribute)
        {
            _methodAttributeSwitch.Switch(method, attribute);
        }

        public void AddBinding(ParameterInfo parameter, object attribute)
        {
            _parameterAttributeSwitch.Switch(parameter, attribute);
        }

        public Function ToFunction()
        {
            return _function;
        }
    }
}
