using Microsoft.Azure.WebJobs;
using QueueTriggerSample.Model;

namespace QueueTriggerSample
{
    public static class Function
    {
        public static void Run(
            [QueueTrigger(queueName: "sample-queue")]Purchase purchase)//,
                                                                       //Logger logger)
        {
            //logger.Info($"Received {purchase.Amount} from {purchase.CustomerName}");
        }
    }
}
