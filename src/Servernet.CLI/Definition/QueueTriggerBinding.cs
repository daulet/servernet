using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class QueueTriggerBinding : IBinding
    {
        internal QueueTriggerBinding(string paramName, QueueTriggerAttribute attribute)
        {
            Name = paramName;
            QueueName = attribute.QueueName;
            ConnectionName = "<Name of app setting that contains a storage connection string>";
            Type = "queueTrigger";
            Direction = "in";
        }

        public string Name { get; }
        public string QueueName { get; }
        public string ConnectionName { get; }
        public string Type { get; }
        public string Direction { get; }
    }
}
