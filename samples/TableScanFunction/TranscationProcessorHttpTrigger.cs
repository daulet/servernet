using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Servernet;
using TableScanFunction.Model;

namespace TableScanFunction
{
    public class TranscationProcessorHttpTrigger
    {
        private readonly IFunction<Transaction, bool> _transactionFunction;

        public TranscationProcessorHttpTrigger(TransactionProcessorFunction transactionFunction)
        {
            _transactionFunction = transactionFunction;
        }

        public async Task<HttpResponseMessage> RunAsync(
            HttpRequestMessage request)
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
