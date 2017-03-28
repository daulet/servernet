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
        private readonly IFunction<DynamicTableEntity, bool> _tableEntityProcessor;

        public TableEntityProcessorHttpTrigger(
            [Inject(typeof(TableEntityProcessorFunction))] IFunction<DynamicTableEntity, bool> tableEntityProcessor)
        {
            _tableEntityProcessor = tableEntityProcessor;
        }

        // @TODO IQueryable, needs to provide async equivalent
        public async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger] HttpRequestMessage request,
            [Table("transaction_table")] IQueryable<DynamicTableEntity> transactionsTable)
        {
            var messageContent = await request.Content.ReadAsStringAsync();
            var entityId = JsonConvert.DeserializeObject<TableEntityId>(messageContent);

            var tableEntity = transactionsTable
                .AsTableQuery()
                .SingleOrDefault(x =>
                    x.PartitionKey == entityId.PartitionKey &&
                    x.RowKey == entityId.RowKey);

            if (tableEntity == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Table entity {entityId.PartitionKey}/{entityId.RowKey} does not exist"),
                };
            }
            else
            {
                var success = _tableEntityProcessor.Run(tableEntity);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"Success: {success}"),
                };
            }
        }
    }
}
