using Microsoft.Azure.WebJobs;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    [AzureFunction]
    public class TransactionPaginationTimerTrigger
    {
        public static void Run(
            [TimerTrigger("0 0 2 * * *")] TimerInfo timer,
            [Queue("transaction_pagination_queue")] ICollector<TableSegment> paginationQueue)
        {
            // queue an empty segment to kick off pagination
            paginationQueue.Add(new TableSegment());
        }
    }
}
