using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Servernet.Generator.Definition
{
    public class WebHookTriggerBinding : IBinding
    {
        internal WebHookTriggerBinding(ParameterInfo parameter, HttpTriggerAttribute attribute)
        {
            AuthLevel = attribute.AuthLevel;
            Name = parameter.Name;
            Route = attribute.Route;
            WebHookType = attribute.WebHookType;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public AuthorizationLevel AuthLevel { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Name { get; }

        public string Route { get; }

        public BindingType Type { get; } = BindingType.HttpTrigger;

        public string WebHookType { get; }
    }
}
