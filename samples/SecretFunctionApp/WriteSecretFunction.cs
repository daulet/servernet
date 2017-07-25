using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Servernet.Extensions.Secret;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Servernet.Samples.SecretFunctionApp
{
    public static class WriteSecretFunction
    {
        [FunctionName("WriteSecretFunction")]
        public static HttpResponseMessage Run(
            HttpRequestMessage req,
            [HttpTrigger("POST", Route = "{containerName}/{secretId}")] byte[] rawData,
            string containerName,
            string secretId,
            [SecretContainer(Container = "{containerName}")] ICollector<Secret> container,
            TraceWriter log)
        {
            log.Info("Received WriteSecretFunction");

            var certificate = new X509Certificate2(rawData);
            var secret = new Secret()
            {
                Certificate = certificate,
                Id = secretId,
            };
            container.Add(secret);

            return req.CreateErrorResponse(HttpStatusCode.OK, $"Received certificate with thumbprint: {secret.Certificate.Thumbprint}");
        }
    }
}