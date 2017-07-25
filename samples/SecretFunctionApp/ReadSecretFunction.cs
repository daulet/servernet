using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Servernet.Extensions.Secret;
using System.Net;
using System.Net.Http;

namespace Servernet.Samples.SecretFunctionApp
{
    public static class ReadSecretFunction
    {
        [FunctionName("ReadSecretFunction")]
        public static HttpResponseMessage Run(
            [HttpTrigger("GET", Route = "{containerName}/{secretId}")] HttpRequestMessage req,
            string containerName,
            string secretId,
            [Secret(Container = "{containerName}", Id = "{secretId}")] string secret,
            TraceWriter log)
        {
            log.Info("Received ReadSecretFunction");
            
            return req.CreateResponse(HttpStatusCode.OK, $"Secret is {secret}");
        }
    }
}