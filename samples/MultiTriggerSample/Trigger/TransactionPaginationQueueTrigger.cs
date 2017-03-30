using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet.Samples.MultiTriggerSample.Function;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    [AzureFunction]
    public class TransactionPaginationQueueTrigger
    {
        public static void Run(
            [QueueTrigger("transaction-pagination-queue")] TableSegment paginationQueueMessage,
            [Queue("transaction-pagination-queue")] ICollector<TableSegment> paginationQueue,
            [Table("transactiontable")] IQueryable<DynamicTableEntity> transactionsTable)
        {
            var paginationFunction = new PaginationFunction(
                new TableEntityProcessorFunction(
                    new TransactionProcessorFunction()));
            paginationFunction.Run(
                paginationQueueMessage,
                paginationQueue,
                transactionsTable);
        }
    }
}
