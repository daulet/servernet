using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using QueueTriggerSample.Model;

namespace QueueTriggerSample
{
    public static class Function
    {
        public static void Run(
            [QueueTrigger(queueName: "sample-queue")]Purchase purchase,
            TraceWriter logger)
        {
            logger.Info($"Received {purchase.Amount} from {purchase.CustomerName}");
        }
    }
}
