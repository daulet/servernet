using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Servernet.Extensions.Secret;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servernet.Samples.SecretFunctionApp
{
    public static class ReadSecretFunction
    {
        [FunctionName("ReadSecretFunction")]
        public static HttpResponseMessage Run(
            [HttpTrigger] HttpRequestMessage req,
            [Secret] string secret,
            TraceWriter log)
        {
            log.Info("Received ReadSecretFunction");
            
            return req.CreateResponse(HttpStatusCode.OK, $"Secret is {secret}");
        }
    }
}