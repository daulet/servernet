using System;
using Microsoft.Azure.WebJobs.Description;

namespace Servernet.Extensions.Azure.KeyVault
{
    [Binding]
    public class SecretAttribute : Attribute
    {
        
    }
}