using System;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Servernet.Samples.PlainSample
{
    public class LogQueueTrigger
    {
        public static void Run(
            [QueueTrigger("plain-queue")] CloudQueueMessage message)
        {
            Console.WriteLine($"Received {message.AsString}");
        }
    }
}
