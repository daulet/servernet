using Microsoft.Azure.WebJobs.Host.Config;

namespace Servernet.Extensions.Secret.Config
{
    public class SecretExtensions : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddConverter<string, string>(x => x);

            var rule = context.AddBindingRule<SecretAttribute>();

            rule.BindToInput<string>(new StringToSecretConverter());
        }
    }
}