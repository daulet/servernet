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
        private readonly TypeSwitch _typeSwitch;

        public FunctionBuilder(
            Assembly functionAssembly,
            Type functionType,
            MethodInfo functionMethod)
        {
            _function = new Function()
            {
                Bindings = new List<IBinding>(),
                Disabled = false,
                EntryPoint = $"{functionType.FullName}.{functionMethod.Name}",
                ScriptFile = $"{Path.GetFileName(functionAssembly.Location)}",
            };
            
            _typeSwitch = new TypeSwitch()
                .Case((ParameterInfo parameter, BlobAttribute x) =>
                {
                    // based on https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob
                    var binding = parameter.IsOut
                        ? (IBinding) new BlobOutputBinding(parameter.Name, x)
                        : (IBinding) new BlobInputBinding(parameter.Name, x);
                    _function.Bindings.Add(binding);
                })
                .Case((ParameterInfo parameter, BlobTriggerAttribute x) => { _function.Bindings.Add(new BlobTriggerBinding(parameter.Name, x)); })
                .Case((ParameterInfo parameter, HttpTriggerAttribute x) => { _function.Bindings.Add(new HttpTriggerBinding(parameter.Name, x)); })
                .Case((ParameterInfo parameter, QueueAttribute x) => { _function.Bindings.Add(new QueueOutputBinding(parameter.Name, x)); })
                .Case((ParameterInfo parameter, QueueTriggerAttribute x) => { _function.Bindings.Add(new QueueTriggerBinding(parameter.Name, x)); })
                .Case((ParameterInfo parameter, SecretAttribute x) => { _function.Bindings.Add(new BlobInputBinding(parameter.Name, x)); })
                .Case((ParameterInfo parameter, TableAttribute x) =>
                {
                    // based on https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-table
                    IBinding binding;
                    if (parameter.IsOut)
                    {
                        binding = new TableOutputBinding(parameter.Name, x);
                    }
                    else
                    {
                        if (typeof(ICollector<>).IsAssignableFrom(parameter.ParameterType) ||
                            typeof(IAsyncCollector<>).IsAssignableFrom(parameter.ParameterType) ||
                            typeof(CloudTable).IsAssignableFrom(parameter.ParameterType))
                        {
                            binding = new TableOutputBinding(parameter.Name, x);
                        }
                        else
                        {
                            binding = new TableInputBinding(parameter.Name, x);
                        }
                    }
                    _function.Bindings.Add(binding);
                });
        }

        public void AddBinding(ParameterInfo parameter, object attribute)
        {
            _typeSwitch.Switch(parameter, attribute);
        }

        public void Validate(ILogger log)
        {
            if (_function.Bindings.OfType<HttpTriggerBinding>().Any())
            {
                if (!_function.Bindings.OfType<HttpOutputBinding>().Any())
                {
                    log.Warning(
                        "If an HTTP output binding is not provided, " +
                        "an HTTP trigger will return HTTP 200 OK with an empty body.");
                }
            }

            if (_function.Bindings.OfType<HttpOutputBinding>().Any())
            {
                if (!_function.Bindings.OfType<HttpTriggerBinding>().Any())
                {
                    log.Error("HttpOutput attribute requires an HTTP trigger " + 
                        "and allows you to customize the response associated with the trigger's request");
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(_function,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                });
        }
    }
}
