using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using System;

namespace Servernet.Extensions.Secret
{
    [Binding]
    public class SecretContainerAttribute : Attribute
    {
        [AppSetting(Default = "SecretStoreConnection")]
        public string Connection { get; set; }

        [AutoResolve]
        public string Container { get; set; }
    }
}
