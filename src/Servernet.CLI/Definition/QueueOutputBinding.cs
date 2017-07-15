using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.Definition
{
    public class QueueOutputBinding : IBinding
    {
        internal QueueOutputBinding(Type functionType, string paramName, QueueAttribute attribute)
        {
            Connection = $"{functionType.Name}_output_queue_{paramName}";
            Name = paramName;
            QueueName = attribute.QueueName;
        }

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.Out;

        public string Name { get; }

        public string QueueName { get; }

        public BindingType Type { get; } = BindingType.Queue;
    }
}
