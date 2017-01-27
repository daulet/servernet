using Microsoft.WindowsAzure.Storage.Queue;

namespace Servernet.SelfHost.Azure.Queue
{
    public interface IsStoredIn<TQueueDefinition> : IInput
        where TQueueDefinition : IQueueDefinition
    {
        CloudQueueMessage GetCloudQueueMessage();
    }
}
