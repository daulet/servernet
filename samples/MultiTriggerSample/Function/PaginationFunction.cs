using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Function
{
    public class PaginationFunction
    {
        private readonly IFunction<DynamicTableEntity, bool> _processor;

        public PaginationFunction(
            IFunction<DynamicTableEntity, bool> processor)
        {
            _processor = processor;
        }

        public void Run(
            TableSegment paginationQueueMessage,
            ICollector<TableSegment> paginationQueue,
            IQueryable<DynamicTableEntity> transactionsTable)
        {
            var tableSegment = transactionsTable
                // @TODO query should be part of input
                .Where(x => x.PartitionKey == "123")
                .AsTableQuery()
                .ExecuteSegmented(paginationQueueMessage.ContinuationToken);

            if (tableSegment.ContinuationToken != null)
            {
                var nextMessage = new TableSegment()
                {
                    ContinuationToken = tableSegment.ContinuationToken
                };
                paginationQueue.Add(nextMessage);
            }

            foreach (var tableEntity in tableSegment.Results)
            {
                var success = _processor.Run(tableEntity);

                if (!success)
                {
                    // @TODO swap out Console with logging implementation
                    Console.WriteLine($"Failed to process {tableEntity.PartitionKey}/{tableEntity.RowKey}");
                }
            }
        }
    }
}
