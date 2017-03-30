using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class QueueTriggerBinding : IBinding
    {
        internal QueueTriggerBinding(Type functionType, string paramName, QueueTriggerAttribute attribute)
        {
            Connection = $"{functionType.Name}_trigger_queue_{paramName}";
            Direction = "in";
            Name = paramName;
            // @TODO Enforce: A queue name can contain only letters, numbers, and and dash(-) characters
            QueueName = attribute.QueueName;
        }

        public string Connection { get; }

        public string Direction { get; }

        public string Name { get; }

        public string QueueName { get; }

        public BindingType Type { get; } = BindingType.QueueTrigger;
    }
}
