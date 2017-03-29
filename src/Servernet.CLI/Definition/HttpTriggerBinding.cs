using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.CLI.Definition
{
    public class HttpTriggerBinding : IBinding
    {
        public HttpTriggerBinding(string paramName, HttpTriggerAttribute attribute)
        {
            AuthLevel = attribute.AuthLevel;
            Direction = "in";
            Methods = attribute.Methods;
            Name = paramName;
            Route = attribute.Route;
            Type = "httpTrigger";
        }

        public HttpAuthLevel AuthLevel { get; set; }

        public string Direction { get; set; }

        public HttpMethod Methods { get; set; }

        public string Name { get; set; }

        public string Route { get; set; }

        public string Type { get; set; }
    }
}
