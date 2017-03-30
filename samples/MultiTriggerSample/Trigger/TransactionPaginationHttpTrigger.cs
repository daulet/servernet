using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TransactionPaginationHttpTrigger
    {
        public static HttpResponseMessage Run(
            [HttpTrigger(HttpMethod.Post, "TransactionPaginationHttpTrigger")] HttpRequestMessage request,
            [Queue("transaction_pagination_queue")] ICollector<TableSegment> paginationQueue)
        {
            // @TODO create queue if does not exist
            // queue an empty segment to kick off pagination
            paginationQueue.Add(new TableSegment());

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
