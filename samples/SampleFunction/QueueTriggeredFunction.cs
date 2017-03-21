using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SampleFunction.Model;
using Servernet;

namespace SampleFunction
{
    public class QueueTriggeredFunction : IFunction
    {
        private readonly Purchase _purchase;

        public QueueTriggeredFunction([QueueTrigger("purchase_queue")] Purchase purchase)
        {
            _purchase = purchase;
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
