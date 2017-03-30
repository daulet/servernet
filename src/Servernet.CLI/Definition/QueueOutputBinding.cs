using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class QueueOutputBinding : IBinding
    {
        internal QueueOutputBinding(Type functionType, string paramName, QueueAttribute attribute)
        {
            Connection = $"{functionType.Name}_output_queue_{paramName}";
            Direction = "out";
            Name = paramName;
            QueueName = attribute.QueueName;
        }

        public string Connection { get; }

        public string Direction { get; }

        public string Name { get; }

        public string QueueName { get; }

        public BindingType Type { get; } = BindingType.Queue;
    }
}
