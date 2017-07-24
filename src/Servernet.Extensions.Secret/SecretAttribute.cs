using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.Extensions.Secret
{
    public class SecretAttribute : Attribute
    {
        [AutoResolve]
        public string SecretId { get; set; }
    }
}