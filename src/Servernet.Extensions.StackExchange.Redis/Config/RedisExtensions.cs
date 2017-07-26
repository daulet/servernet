using Microsoft.Azure.WebJobs.Host.Config;

namespace Servernet.Extensions.StackExchange.Redis.Config
{
    public class RedisExtensions : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var secretAttributeRules = context.AddBindingRule<RedisAttribute>();
            secretAttributeRules.BindToInput(new SecretToRedisConverter());
        }
    }
}
