using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Azure.WebJobs;
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
                    IBinding binding = parameter.IsOut
                        ? (IBinding) new BlobOutputBinding(parameter.Name, x)
                        : (IBinding) new BlobInputBinding(parameter.Name, x);
                    _function.Bindings.Add(binding);
                })
                .Case((ParameterInfo parameter, BlobTriggerAttribute x) => { _function.Bindings.Add(new BlobTriggerBinding(parameter.Name, x)); })
                .Case((ParameterInfo parameter, QueueAttribute x) => { _function.Bindings.Add(new QueueBinding(parameter.Name, x)); })
                .Case((ParameterInfo parameter, QueueTriggerAttribute x) => { _function.Bindings.Add(new QueueTriggerBinding(parameter.Name, x)); });
        }

        public void AddBinding(ParameterInfo parameter, object attribute)
        {
            _typeSwitch.Switch(parameter, attribute);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(_function,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}
