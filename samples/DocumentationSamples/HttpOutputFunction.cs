using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class HttpOutputFunction
    {
        [FunctionName("HttpOutputFunction")]
        [HttpOutput]
        public static async Task<HttpResponseMessage> Run(
               [HttpTrigger("GET", Route = "HttpOutputFunction/{subPath:alpha}")] HttpRequestMessage req,
               string subPath,
               TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}, subPath={subPath}");

            // parse query parameter
            var name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            name = name ?? data?.name;

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }
}
