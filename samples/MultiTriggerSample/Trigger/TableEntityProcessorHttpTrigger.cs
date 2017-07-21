using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using Servernet.Samples.MultiTriggerSample.Function;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TableEntityProcessorHttpTrigger
    {
        [FunctionName("TableEntityProcessorHttpTrigger")]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger("POST", Route = "TableEntityProcessorHttpTrigger/{partitionKey}/{rowKey}")] HttpRequestMessage request,
            string partitionKey,
            string rowKey,
            [Table("transactiontable")] IQueryable<DynamicTableEntity> transactionsTable)
        {
            var tableSegment = await transactionsTable
                .Where(x =>
                    x.PartitionKey == partitionKey &&
                    x.RowKey == rowKey)
                .AsTableQuery()
                .ExecuteSegmentedAsync(currentToken: null);

            var tableEntity = tableSegment.Results.SingleOrDefault();

            if (tableEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"Table entity {partitionKey}/{rowKey} does not exist"),
                };
            }
            else
            {
                var tableEntityProcessor = new TableEntityProcessorFunction(
                    new TransactionProcessorFunction());
                var success = tableEntityProcessor.Run(tableEntity);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"Success: {success}"),
                };
            }
        }
    }
}
