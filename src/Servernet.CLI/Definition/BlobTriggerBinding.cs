using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class BlobTriggerBinding : IBinding
    {
        internal BlobTriggerBinding(Type functionType, string paramName, BlobTriggerAttribute attribute)
        {
            Connection = $"{functionType.Name}_trigger_blob_{paramName}";
            Name = paramName;
            Path = attribute.BlobPath;
        }

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Name { get; }

        public string Path { get; }

        public BindingType Type { get; } = BindingType.BlobTrigger;
    }
}
