﻿using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using System;

namespace Servernet.Extensions.Secret
{
    [Binding]
    public class SecretAttribute : Attribute
    {
        [AppSetting(Default = "SecretStoreConnection")]
        public string Connection { get; set; }

        [AutoResolve]
        public string Container { get; set; }

        [AutoResolve]
        public string Id { get; set; }
    }
}