using System;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SecretAttribute : Attribute
    {
        public SecretAttribute(string secretPath)
        {
            Path = secretPath;
        }

        public string Path { get; private set; }
    }
}
