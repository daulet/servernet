using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;

namespace Servernet.Samples.DocumentationSamples
{
    public class WebHookTriggerFunction
    {
        [FunctionName("WebHookTriggerFunction")]
        [HttpOutput]
        public static HttpResponseMessage Run(
            [HttpTrigger(Route = "products/{category:alpha}/{id:int?}", WebHookType = "genericJson")] HttpRequestMessage request,
            string category,
            int? id)
        {
            if (id == null)
                return request.CreateResponse(HttpStatusCode.OK, $"All {category} items were requested.");
            else
                return request.CreateResponse(HttpStatusCode.OK, $"{category} item with id = {id} has been requested.");
        }
    }
}
