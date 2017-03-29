using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Servernet.CLI.Definition
{
    public class HttpTriggerBinding : IBinding
    {
        public HttpTriggerBinding(string paramName, HttpTriggerAttribute attribute)
        {
            AuthLevel = attribute.AuthLevel;
            Direction = "in";
            Methods = new List<string>();
            foreach (HttpMethod method in Enum.GetValues(typeof(HttpMethod)))
            {
                if ((attribute.Methods & method) != 0)
                {
                    Methods.Add(method.ToString().ToUpperInvariant());
                }
            }
            Name = paramName;
            Route = attribute.Route;
            Type = "httpTrigger";
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public HttpAuthLevel AuthLevel { get; set; }

        public string Direction { get; set; }

        public List<string> Methods { get; set; }

        public string Name { get; set; }

        public string Route { get; set; }

        public string Type { get; set; }
    }
}
