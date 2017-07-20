using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.SoapEndpointSample
{
    public class SoapHandler
    {
        [HttpOutput]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger("GET", "POST", Route = "ols/olsclient.svc/olsclient")] HttpRequestMessage request,
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