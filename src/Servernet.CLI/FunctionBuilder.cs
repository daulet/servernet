using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Servernet.CLI.Definition;

namespace Servernet.CLI
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

            var attribute = functionType.GetCustomAttribute<AzureFunctionAttribute>(inherit: false);
            if (attribute != null)
            {
                _function.Disabled = attribute.Disabled;
                _function.Name = attribute.Name?? functionType.Name;
            }

            _methodAttributeSwitch = new TypeSwitch<MethodInfo>()
                .Case((MethodInfo method, HttpResponseAttribute x) => { _function.Bindings.Add(new HttpOutputBinding("$return")); });

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
                .Case((ParameterInfo parameter, HttpTriggerAttribute x) => { _function.Bindings.Add(new HttpTriggerBinding(parameter.Name, x)); })
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
                        if (typeof(ICollector<>).IsAssignableFrom(parameter.ParameterType) ||
                            typeof(IAsyncCollector<>).IsAssignableFrom(parameter.ParameterType) ||
                            typeof(CloudTable).IsAssignableFrom(parameter.ParameterType))
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
                .Case((ParameterInfo parameter, WebHookTriggerAttribute x) => { _function.Bindings.Add(new WebHookTriggerBinding(parameter, x)); });
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
