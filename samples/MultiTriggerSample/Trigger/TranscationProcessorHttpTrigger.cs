using System.Net;
using System.Net.Http;
using Servernet.Samples.MultiTriggerSample.Function;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    [AzureFunction]
    public class TranscationProcessorHttpTrigger
    {
        public static HttpResponseMessage Run(
            [HttpTrigger(HttpMethod.Post, "TranscationProcessorHttpTrigger")] Transaction transaction)
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
