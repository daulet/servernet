using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using System;

namespace Servernet.Extensions.StackExchange.Redis
{
    [Binding]
    public class RedisAttribute : Attribute
    {
        /// <summary>
        /// Allows Redis config to be defined in app settings
        /// </summary>
        [AppSetting(Default = "RedisConfigurationOptions")]
        public string Configuration { get; set; }
    }
}