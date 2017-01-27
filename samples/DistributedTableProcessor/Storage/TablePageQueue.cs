using DistributedTableProcessor.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Servernet.SelfHost.Azure.Queue;

namespace DistributedTableProcessor.Storage
{
    public class TablePageQueue : IQueueDefinition<TablePage>
    {
        public CloudStorageAccount Account { get; } = CloudStorageAccount.DevelopmentStorageAccount;

        public string Name { get; } = "TablePageQueue";

        public TablePage GetEntity(CloudQueueMessage cloudQueueMessage)
        {
            return new TablePage(cloudQueueMessage);
        }
    }
}
