using System;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class WebhookTrigger : Attribute
    {
        public WebhookTrigger(string route, WebhookType type)
        {
            Route = route;
            Type = type;
        }

        public HttpAuthLevel AuthLevel { get; set; } = HttpAuthLevel.Anonymous;
        
        public string Route { get; }

        public WebhookType Type { get; }
    }
}
