using System;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Servernet.Samples.PlainSample
{
    public class LogQueueTrigger
    {
        public void Run(
            [QueueTrigger("plain_queue")] CloudQueueMessage message)
        {
            Console.WriteLine($"Received {message.AsString}");
        }
    }
}
