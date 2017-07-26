using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using StackExchange.Redis;

namespace Servernet.Extensions.StackExchange.Redis.Config
{
    public class SecretToRedisConverter : IAsyncConverter<RedisAttribute, IConnectionMultiplexer>
    {
        public async Task<IConnectionMultiplexer> ConvertAsync(RedisAttribute input, CancellationToken cancellationToken)
        {
            var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(input.Configuration);

            return connectionMultiplexer;
        }
    }
}