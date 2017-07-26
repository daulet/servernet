using System;
using Microsoft.Azure.WebJobs;
using Servernet.Extensions.StackExchange.Redis;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Servernet.Samples.RedisFunctionApp
{
    public static class ListKeysFunction
    {
        /// <example>GET .../api/cache?pattern=test*</example>
        [FunctionName(nameof(ListKeysFunction))]
        public static IEnumerable<string> Run(
            [HttpTrigger("GET", Route = "cache")] HttpRequestMessage req,
            [Redis] IConnectionMultiplexer connectionMultiplexer)
        {
            var pattern = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Equals(
                    q.Key, "pattern", StringComparison.OrdinalIgnoreCase))
                .Value;

            var randomEndpoint = connectionMultiplexer.GetEndPoints().First();
            var keys = connectionMultiplexer.GetServer(randomEndpoint).Keys(pattern: pattern);

            foreach (var key in keys)
            {
                yield return key;
            }
        }
    }
}