using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Servernet.Generator.Definition
{
    public class HttpTriggerBinding : IBinding
    {
        internal HttpTriggerBinding(string paramName, HttpTriggerAttribute attribute)
        {
            AuthLevel = attribute.AuthLevel;
            Methods = new List<string>(attribute.Methods);
            Name = paramName;
            // @TODO Enforce: The route template cannot start with a '/' or '~' character and it cannot contain a '?' character.
            // @TODO Validate that dynamically assigned route parameters are present in route template
            Route = attribute.Route;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public AuthorizationLevel AuthLevel { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public List<string> Methods { get; }

        public string Name { get; }

        public string Route { get; }

        public BindingType Type { get; } = BindingType.HttpTrigger;
    }
}
