using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Servernet.CLI.Definition
{
    public class HttpTriggerBinding : IBinding
    {
        internal HttpTriggerBinding(string paramName, HttpTriggerAttribute attribute)
        {
            AuthLevel = attribute.AuthLevel;
            Methods = new List<string>();
            foreach (HttpMethod method in Enum.GetValues(typeof(HttpMethod)))
            {
                if ((attribute.Methods & method) != 0)
                {
                    Methods.Add(method.ToString().ToUpperInvariant());
                }
            }
            Name = paramName;
            // @TODO Enforce: The route template cannot start with a '/' or '~' character and it cannot contain a '?' character.
            // @TODO Validate that dynamically assigned route parameters are present in route template
            Route = attribute.Route;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public HttpAuthLevel AuthLevel { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public List<string> Methods { get; }

        public string Name { get; }

        public string Route { get; }

        public BindingType Type { get; } = BindingType.HttpTrigger;
    }
}
