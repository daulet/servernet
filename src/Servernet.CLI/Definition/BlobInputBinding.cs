using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class BlobInputBinding : IBinding
    {
        public BlobInputBinding(string paramName, BlobAttribute attribute)
        {
            Connection = "<Name of app setting that contains a storage connection string>";
            Direction = "in";
            Name = paramName;
            Path = attribute.BlobPath;
            Type = "blob";
        }

        public BlobInputBinding(string paramName, SecretAttribute attribute)
        {
            Connection = "<Name of app setting that contains a storage connection string>";
            Direction = "in";
            Name = paramName;
            Path = attribute.Path;
            Type = "blob";
        }

        public string Connection { get; }
        public string Direction { get; }
        public string Name { get; }
        public string Path { get; }
        public string Type { get; }
    }
}
