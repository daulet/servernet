using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Servernet.Generator.Definition
{
    public class WebHookTriggerBinding : IBinding
    {
        internal WebHookTriggerBinding(ParameterInfo parameter, WebHookTriggerAttribute attribute)
        {
            AuthLevel = attribute.AuthLevel;
            Name = parameter.Name;
            Route = attribute.Route;
            WebHookType = attribute.Type;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public HttpAuthLevel AuthLevel { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Name { get; }

        public string Route { get; }

        public BindingType Type { get; } = BindingType.HttpTrigger;

        public WebhookType WebHookType { get; }
    }
}
