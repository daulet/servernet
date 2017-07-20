using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class HttpTriggerFunction
    {
        public static async Task Run(
            [HttpTrigger(HttpMethod.Get, "HttpTriggerFunction")] HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

            // parse query parameter
            var name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            name = name ?? data?.name;

            if (name == null)
            {
                log.Error("Missing name on the query string or in the request body");
            }
            else
            {
                log.Info($"Received request from {name}");
            }
        }
    }
}
