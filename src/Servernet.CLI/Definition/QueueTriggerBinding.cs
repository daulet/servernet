using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.Definition
{
    public class QueueTriggerBinding : IBinding
    {
        internal QueueTriggerBinding(Type functionType, string paramName, QueueTriggerAttribute attribute)
        {
            Connection = $"{functionType.Name}_trigger_queue_{paramName}";
            Name = paramName;
            // @TODO Enforce: A queue name can contain only letters, numbers, and and dash(-) characters
            QueueName = attribute.QueueName;
        }

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Name { get; }

        public string QueueName { get; }

        public BindingType Type { get; } = BindingType.QueueTrigger;
    }
}
