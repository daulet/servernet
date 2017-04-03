using System.Net;
using System.Net.Http;

namespace Servernet.Samples.DocumentationSamples
{
    [AzureFunction]
    public class WebHookTriggerFunction
    {
        [HttpOutput]
        public static HttpResponseMessage Run(
            [WebHookTrigger("products/{category:alpha}/{id:int?}", WebhookType.GenericJson)] HttpRequestMessage request,
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
