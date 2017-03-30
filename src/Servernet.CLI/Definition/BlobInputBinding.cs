using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class BlobInputBinding : IBinding
    {
        public BlobInputBinding(Type functionType, string paramName, BlobAttribute attribute)
        {
            Connection = $"{functionType.Name}_input_blob_{paramName}";
            Direction = "in";
            Name = paramName;
            Path = attribute.BlobPath;
        }

        public BlobInputBinding(Type functionType, string paramName, SecretAttribute attribute)
        {
            Connection = $"{functionType.Name}_input_blob_{paramName}";
            Direction = "in";
            Name = paramName;
            Path = attribute.Path;
        }

        public string Connection { get; }

        public string Direction { get; }

        public string Name { get; }

        public string Path { get; }

        public BindingType Type { get; } = BindingType.Blob;
    }
}
