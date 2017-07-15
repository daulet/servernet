using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.Definition
{
    public class BlobInputBinding : IBinding
    {
        internal BlobInputBinding(Type functionType, string paramName, BlobAttribute attribute)
        {
            Connection = $"{functionType.Name}_input_blob_{paramName}";
            Name = paramName;
            Path = attribute.BlobPath;
        }

        internal BlobInputBinding(Type functionType, string paramName, SecretAttribute attribute)
        {
            Connection = $"{functionType.Name}_input_blob_{paramName}";
            Name = paramName;
            Path = attribute.Path;
        }

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Name { get; }

        public string Path { get; }

        public BindingType Type { get; } = BindingType.Blob;
    }
}
