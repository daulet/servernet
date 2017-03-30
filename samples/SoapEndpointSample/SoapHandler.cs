using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.SoapEndpointSample
{
    [AzureFunction]
    public class SoapHandler
    {
        [HttpResponse]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(HttpMethod.Get | HttpMethod.Post, "ols/olsclient.svc/olsclient")] HttpRequestMessage request,
            TraceWriter logger)
        {
            var content = await request.Content.ReadAsStringAsync();
            return new HttpResponseMessage()
            {
                Content = new StringContent(content),
            };
        }
    }
}