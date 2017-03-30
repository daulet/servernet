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
            [HttpTrigger(HttpMethod.Post, "TableEntityProcessorHttpTrigger")] HttpRequestMessage request,
            [Table("transactiontable")] IQueryable<DynamicTableEntity> transactionsTable)
        {
            var messageContent = await request.Content.ReadAsStringAsync();
            var entityId = JsonConvert.DeserializeObject<TableEntityId>(messageContent);

            var tableEntities = transactionsTable
                .Where(x =>
                    x.PartitionKey == entityId.PartitionKey &&
                    x.RowKey == entityId.RowKey)
                .AsTableQuery()
                .Execute();

            var tableEntity = tableEntities.SingleOrDefault();

            if (tableEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Table entity {entityId.PartitionKey}/{entityId.RowKey} does not exist"),
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
