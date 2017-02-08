using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using System;

namespace Servernet
{
    public static class LoggingJobHostConfigurationExtensions
    {
        public static void UseLogging(this JobHostConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            // Register our extension configuration provider
            config.RegisterExtensionConfigProvider(new LoggingExtensionConfig());
        }

        private class LoggingExtensionConfig : IExtensionConfigProvider
        {
            public void Initialize(ExtensionConfigContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                // Register our extension binding providers
                context.Config.RegisterBindingExtensions(
                    new LoggingAttributeBindingProvider());
            }
        }
    }
}
