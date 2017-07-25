using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;

namespace Servernet.Extensions.Secret
{
    [Binding]
    public class SecretAttribute : Attribute
    {
        [AppSetting(Default = "SecretStoreConnection")]
        public string Connection { get; set; }

        [AutoResolve]
        public string SecretId { get; set; }
    }
}