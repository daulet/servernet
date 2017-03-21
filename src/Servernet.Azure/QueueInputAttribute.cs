using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.Azure
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class QueueInputAttribute : Attribute
    {
        private readonly string _queueName;

        public QueueInputAttribute(string queueName)
        {
            _queueName = queueName;
        }
    }
}
