using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class BlobOutputBinding : IBinding
    {
        internal BlobOutputBinding(Type functionType, string paramName, BlobAttribute attribute)
        {
            Connection = $"{functionType.Name}_output_blob_{paramName}";
            Name = paramName;
            Path = attribute.BlobPath;
        }

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.Out;

        public string Name { get; }

        public string Path { get; }

        public BindingType Type { get; } = BindingType.Blob;
    }
}
