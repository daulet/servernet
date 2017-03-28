using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Servernet.CLI.Definition;

namespace Servernet.CLI
{
    internal class FunctionBuilder
    {
        private readonly Function _function;
        private readonly TypeSwitch _typeSwitch;

        public FunctionBuilder()
        {
            _function = new Function()
            {
                Disabled = false,
                Bindings = new List<IBinding>(),
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
            return JsonConvert.SerializeObject(_function, Formatting.Indented);
        }
    }
}
