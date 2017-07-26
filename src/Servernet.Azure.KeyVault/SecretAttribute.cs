using System;
using Microsoft.Azure.WebJobs.Description;

namespace Servernet.Azure.KeyVault
{
    [Binding]
    public class SecretAttribute : Attribute
    {
        
    }
}