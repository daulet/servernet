using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class QueueTriggerFunction
    {
        public static void Run(
            [QueueTrigger("myqueue-items")]string myQueueItem,
            DateTimeOffset expirationTime,
            DateTimeOffset insertionTime,
            DateTimeOffset nextVisibleTime,
            string queueTrigger,
            string id,
            string popReceipt,
            int dequeueCount,
            TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}\n" +
                $"queueTrigger={queueTrigger}\n" +
                $"expirationTime={expirationTime}\n" +
                $"insertionTime={insertionTime}\n" +
                $"nextVisibleTime={nextVisibleTime}\n" +
                $"id={id}\n" +
                $"popReceipt={popReceipt}\n" +
                $"dequeueCount={dequeueCount}");
        }
    }
}