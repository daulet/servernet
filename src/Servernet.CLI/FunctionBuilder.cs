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
                .Case((string paramName, QueueTriggerAttribute x) => { _function.Bindings.Add(new QueueTriggerBinding(paramName, x)); })
                ;
        }

        public void AddBinding(ParameterInfo parameter, object attribute)
        {
            _typeSwitch.Switch(parameter.Name, attribute);
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
