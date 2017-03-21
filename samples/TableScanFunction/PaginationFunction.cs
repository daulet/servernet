using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using Servernet;
using Servernet.Azure;
using TableScanFunction.Model;

namespace TableScanFunction
{
    public class PaginationFunction : IFunction
    {
        private readonly TableSegment _paginationQueueMessage;
        private readonly IQueue<TableSegment> _paginationQueue;
        private readonly IQueryable<DynamicTableEntity> _transactionsTable;

        public PaginationFunction(
            [QueueInput("pagination_queue")]TableSegment paginationQueueMessage,
            [QueueOutput("pagination_queue")]IQueue<TableSegment> paginationQueue,
            [TableInput("transactions_table")] IQueryable<DynamicTableEntity> transactionsTable)
        {
            _paginationQueueMessage = paginationQueueMessage;
            _paginationQueue = paginationQueue;
            _transactionsTable = transactionsTable;
        }

        public void Run()
        {
            var tableSegment = _transactionsTable
                .Where(x => x.PartitionKey == "123")
                .AsTableQuery()
                .ExecuteSegmented(_paginationQueueMessage.ContinuationToken);

            if (tableSegment.ContinuationToken != null)
            {
                var nextMessage = new TableSegment()
                {
                    ContinuationToken = tableSegment.ContinuationToken
                };
                _paginationQueue.AddMessage(nextMessage);
            }

            foreach (var tableEntity in tableSegment.Results)
            {
                Console.WriteLine($"Fetched {tableEntity.PartitionKey}/{tableEntity.RowKey}");
            }
        }
    }
}
