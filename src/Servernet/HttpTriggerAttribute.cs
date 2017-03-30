using System;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class HttpTriggerAttribute : Attribute
    {
        public HttpTriggerAttribute(HttpMethod httpMethods, string route)
        {
            Methods = httpMethods;
            Route = route;
        }

        public HttpAuthLevel AuthLevel { get; set; } = HttpAuthLevel.Anonymous;

        public HttpMethod Methods { get; }

        public string Route { get; }
    }
}
