using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using TableScanFunction.Model;

namespace TableScanFunction
{
    public static class TransactionPaginationTimerTrigger
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
