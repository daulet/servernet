using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;
using Newtonsoft.Json;
using Servernet.Samples.MultiTriggerSample.Function;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Trigger
{
    public class TableEntityProcessorHttpTrigger
    {
        // @TODO IQueryable, needs to provide async equivalent
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(HttpMethod.Post, "TableEntityProcessorHttpTrigger/{partitionKey}/{rowKey}")] HttpRequestMessage request,
            string partitionKey,
            string rowKey,
            [Table("transactiontable")] IQueryable<DynamicTableEntity> transactionsTable)
        {
            var tableEntities = transactionsTable
                .Where(x =>
                    x.PartitionKey == partitionKey &&
                    x.RowKey == rowKey)
                .AsTableQuery()
                .Execute();

            var tableEntity = tableEntities.SingleOrDefault();

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
