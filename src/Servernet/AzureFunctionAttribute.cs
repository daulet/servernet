using System;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AzureFunctionAttribute : Attribute
    {
        public bool Disabled { get; set; }

        public string Name { get; set; }
    }
}
