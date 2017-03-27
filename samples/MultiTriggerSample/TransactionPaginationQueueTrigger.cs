using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet;
using TableScanFunction.Model;

namespace TableScanFunction
{
    public class TransactionPaginationQueueTrigger
    {
        private readonly PaginationFunction _paginationFunction;

        public TransactionPaginationQueueTrigger(PaginationFunction paginationFunction)
        {
            _paginationFunction = paginationFunction;
        }

        public void Run(
            [QueueTrigger("transaction_pagination_queue")] TableSegment paginationQueueMessage,
            [Queue("transaction_pagination_queue")] ICollector<TableSegment> paginationQueue,
            [Table("transaction_table")] IQueryable<DynamicTableEntity> transactionsTable)
        {
            _paginationFunction.Run(
                paginationQueueMessage,
                paginationQueue,
                transactionsTable);
        }
    }
}
