using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Servernet.SelfHost.Azure.Queue
{
    public interface IQueueDefinition<TEntity>
        where TEntity : IQueueEntity
    {
        CloudStorageAccount Account { get; }

        string Name { get; }

        TEntity GetEntity(CloudQueueMessage cloudQueueMessage);
    }
}
