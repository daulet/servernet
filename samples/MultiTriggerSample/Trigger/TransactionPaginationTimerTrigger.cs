﻿using Microsoft.Azure.WebJobs;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TransactionPaginationTimerTrigger
    {
        [FunctionName("TransactionPaginationTimerTrigger")]
        public static void Run(
            [TimerTrigger("0 0 2 * * *")] TimerInfo timer,
            [Queue("transaction-pagination-queue")] ICollector<TableSegment> paginationQueue)
        {
            // queue an empty segment to kick off pagination
            paginationQueue.Add(new TableSegment());
        }
    }
}
