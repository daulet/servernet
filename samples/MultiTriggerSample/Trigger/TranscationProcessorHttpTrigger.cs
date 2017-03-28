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
        private readonly IFunction<Transaction, bool> _transactionFunction;

        public TranscationProcessorHttpTrigger(
            [Inject] TransactionProcessorFunction transactionFunction)
        {
            _transactionFunction = transactionFunction;
        }

        public async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger] HttpRequestMessage request)
        {
            var messageContent = await request.Content.ReadAsStringAsync();
            var transaction = JsonConvert.DeserializeObject<Transaction>(messageContent);

            var success = _transactionFunction.Run(transaction);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent($"Success: {success}"),
            };
        }
    }
}
