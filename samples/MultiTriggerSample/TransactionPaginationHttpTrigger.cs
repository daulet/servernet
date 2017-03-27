using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using TableScanFunction.Model;

namespace TableScanFunction
{
    public static class TransactionPaginationHttpTrigger
    {
        public static HttpResponseMessage Run(
            HttpRequestMessage request,
            [Queue("transaction_pagination_queue")] ICollector<TableSegment> paginationQueue)
        {
            // queue an empty segment to kick off pagination
            paginationQueue.Add(new TableSegment());

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
