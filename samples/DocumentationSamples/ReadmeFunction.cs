using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.DocumentationSamples
{
    public class ReadmeFunction
    {
        [FunctionName("ReadmeFunction")]
        [HttpOutput]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger("POST", Route = "users/{username:alpha}")] HttpRequestMessage request,
            string username,
            [Table("User", partitionKey: "{username}", rowKey: "payment")] PaymentInstrument paymentInstrument,
            [Queue("user-queue")] IAsyncCollector<PaymentInstrument> paymentQueue,
            TraceWriter log)
        {
            log.Info($"Queuing payment for user {username}");

            await paymentQueue.AddAsync(paymentInstrument);

            return new HttpResponseMessage()
            {
                Content = new StringContent($"Queued payment for user {username}"),
            };
        }

        public class PaymentInstrument
        {
            public string CreditCardNumber { get; set; }

            public DateTimeOffset ExpirationDate { get; set; }

            public string FullName { get; set; }
        }
    }
}