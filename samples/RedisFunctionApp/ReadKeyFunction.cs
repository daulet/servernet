using Microsoft.Azure.WebJobs;
using Servernet.Extensions.StackExchange.Redis;
using StackExchange.Redis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servernet.Samples.RedisFunctionApp
{
    public static class ReadKeyFunction
    {
        /// <example>GET .../api/cache/test</example>
        [FunctionName(nameof(ReadKeyFunction))]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger("GET", Route = "cache/{key}")] HttpRequestMessage req,
            string key,
            [Redis] IConnectionMultiplexer connectionMultiplexer)
        {
            var value = await connectionMultiplexer.GetDatabase().StringGetAsync(key);

            return req.CreateResponse(HttpStatusCode.OK, $"{key} = {value}");
        }
    }
}
