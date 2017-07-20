using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TransactionPaginationHttpTrigger
    {
        [FunctionName("TriggerProcessingTransactionTable")]
        public static HttpResponseMessage Run(
            [HttpTrigger("POST")] HttpRequestMessage request,
            [Queue("transaction-pagination-queue")] ICollector<TableSegment> paginationQueue)
        {
            // queue an empty segment to kick off pagination
            paginationQueue.Add(new TableSegment());

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
