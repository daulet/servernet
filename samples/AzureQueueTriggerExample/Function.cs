using AzureQueueTriggerExample.Model;
using Microsoft.Azure.WebJobs;
using Servernet;

namespace AzureQueueTriggerExample
{
    public class Function
    {
        public static void Run(
            [QueueTrigger(queueName: "sample-queue")]Purchase purchase,
            Logger logger)
        {
            logger.Info($"Received {purchase.Amount} from {purchase.CustomerName}");
        }
    }
}
