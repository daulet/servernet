using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class BlobOutputBinding : IBinding
    {
        public BlobOutputBinding(Type functionType, string paramName, BlobAttribute attribute)
        {
            Connection = $"{functionType.Name}_output_blob_{paramName}";
            Direction = "out";
            Name = paramName;
            Path = attribute.BlobPath;
            Type = "blob";
        }

        public string Connection { get; }
        public string Direction { get; }
        public string Name { get; }
        public string Path { get; }
        public string Type { get; }
    }
}
