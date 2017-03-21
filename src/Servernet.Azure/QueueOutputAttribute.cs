using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.Azure
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class QueueOutputAttribute : Attribute
    {
        private readonly string _queueName;

        public QueueOutputAttribute(string queueName)
        {
            _queueName = queueName;
        }
    }
}
