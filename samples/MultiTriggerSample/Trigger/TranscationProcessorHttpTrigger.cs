using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Servernet.Samples.MultiTriggerSample.Function;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TranscationProcessorHttpTrigger
    {
        public static async Task<HttpResponseMessage> RunAsync(
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
