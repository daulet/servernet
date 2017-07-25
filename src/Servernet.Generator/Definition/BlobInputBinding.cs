using Microsoft.Azure.WebJobs;
using System;

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

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Name { get; }

        public string Path { get; }

        public BindingType Type { get; } = BindingType.Blob;
    }
}
