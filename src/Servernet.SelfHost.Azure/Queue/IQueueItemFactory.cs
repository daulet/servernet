using Microsoft.WindowsAzure.Storage.Queue;

namespace Servernet.SelfHost.Azure.Queue
{
    public interface IQueueItemFactory<TQueue>
        where TQueue : IQueueDefinition
    {
        IsStoredIn<TQueue> CreateQueueItem(CloudQueueMessage message);
    }
}
