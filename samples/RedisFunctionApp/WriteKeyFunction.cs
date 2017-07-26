using Microsoft.Azure.WebJobs;
using Servernet.Extensions.StackExchange.Redis;
using StackExchange.Redis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servernet.Samples.RedisFunctionApp
{
    public static class WriteKeyFunction
    {
        /// <example>POST .../api/cache/test</example>
        [FunctionName(nameof(WriteKeyFunction))]
        public static async Task<HttpResponseMessage> RunAsync(
            HttpRequestMessage req,
            [HttpTrigger("POST", Route = "cache/{key}")] string value,
            string key,
            [Redis] IConnectionMultiplexer connectionMultiplexer)
        {
            var result = await connectionMultiplexer.GetDatabase().StringSetAsync(key, value);

            return req.CreateResponse(HttpStatusCode.OK, $"Setting {key} = {value}. Success = {result}");
        }
    }
}
