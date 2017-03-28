﻿using System;
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
            Connection = "<Name of app setting that contains a storage connection string>";
            Direction = "in";
            Name = paramName;
            QueueName = attribute.QueueName;
            Type = "queueTrigger";
        }

        public string Connection { get; }
        public string Direction { get; }
        public string Name { get; }
        public string QueueName { get; }
        public string Type { get; }
    }
}
