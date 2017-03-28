using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class QueueOutputBinding : IBinding
    {
        internal QueueOutputBinding(string paramName, QueueAttribute attribute)
        {
            Connection = "<Name of app setting that contains a storage connection string>";
            Direction = "out";
            Name = paramName;
            QueueName = attribute.QueueName;
            Type = "queue";
        }

        public string Connection { get; }
        public string Direction { get; }
        public string Name { get; }
        public string QueueName { get; }
        public string Type { get; }
    }
}
