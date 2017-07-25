using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Servernet.Extensions.Secret;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servernet.Samples.SecretFunctionApp
{
    public static class WriteSecretFunction
    {
        [FunctionName("WriteSecretFunction")]
        public static HttpResponseMessage Run(
            HttpRequestMessage req,
            [HttpTrigger("POST", Route = "{containerName}/{secretId}")] string secretToUpload,
            string containerName,
            string secretId,
            [SecretContainer(Container = "{containerName}")] ICollector<Secret> container,
            TraceWriter log)
        {
            container.Add(new Secret()
            {
                EncodedSecret = secretToUpload,
                Id = secretId,
            });
            return req.CreateErrorResponse(HttpStatusCode.OK, $"Secret is {secretToUpload}");
        }
    }
}