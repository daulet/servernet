using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Servernet.Extensions.Secret;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Servernet.Samples.SecretFunctionApp
{
    public static class ReadSecretFunction
    {
        [FunctionName("ReadSecretFunction")]
        public static HttpResponseMessage Run(
            [HttpTrigger("GET", Route = "{containerName}/{secretId}")] HttpRequestMessage req,
            string containerName,
            string secretId,
            [Secret(Container = "{containerName}", Id = "{secretId}")] X509Certificate2 certificate,
            TraceWriter log)
        {
            log.Info("Received ReadSecretFunction");
            
            return req.CreateResponse(HttpStatusCode.OK, $"Secret's thumbprint is {certificate.Thumbprint}");
        }
    }
}