using Microsoft.Azure.WebJobs;
using Servernet.Samples.MultiTriggerSample.Function;
using Servernet.Samples.MultiTriggerSample.Model;
using System.Net;
using System.Net.Http;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TranscationProcessorHttpTrigger
    {
        [FunctionName("TranscationProcessorHttpTrigger")]
        public static HttpResponseMessage Run(
            [HttpTrigger("POST")] Transaction transaction)
        {
            IFunction<Transaction, bool> transactionFunction = new TransactionProcessorFunction();
            var success = transactionFunction.Run(transaction);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"Success: {success}"),
            };
        }
    }
}
