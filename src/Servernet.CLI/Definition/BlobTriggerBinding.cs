using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class BlobTriggerBinding : IBinding
    {
        public BlobTriggerBinding(Type functionType, string paramName, BlobTriggerAttribute attribute)
        {
            Connection = $"{functionType.Name}_trigger_blob_{paramName}";
            Direction = "in";
            Name = paramName;
            Path = attribute.BlobPath;
            Type = "blobTrigger";
        }

        public string Connection { get; }
        public string Direction { get; }
        public string Name { get; }
        public string Path { get; }
        public string Type { get; }
    }
}
