using Microsoft.Azure.WebJobs.Host.Config;

namespace Servernet.Extensions.Secret.Config
{
    public class SecretExtensions : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddConverter<string, string>(x => x);

            var secretAttributeRules = context.AddBindingRule<SecretAttribute>();
            secretAttributeRules.BindToInput<string>(new BlobToCertificateConverter());

            var secretContainerAttributeRules = context.AddBindingRule<SecretContainerAttribute>();
            secretContainerAttributeRules.BindToCollector(attribute => new SecretToBlobCollector(attribute));
        }
    }
}