using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleFunction
{
    public class AFunction : IFunction
    {
        private readonly string _queueMessage;

        public AFunction([QueueTrigger("sample_queue")] string queueMessage)
        {
            _queueMessage = queueMessage;
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
