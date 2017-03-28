using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet.Samples.MultiTriggerSample.Function;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TransactionPaginationQueueTrigger
    {
        public static void Run(
            [QueueTrigger("transaction_pagination_queue")] TableSegment paginationQueueMessage,
            [Queue("transaction_pagination_queue")] ICollector<TableSegment> paginationQueue,
            [Table("transaction_table")] IQueryable<DynamicTableEntity> transactionsTable)
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
