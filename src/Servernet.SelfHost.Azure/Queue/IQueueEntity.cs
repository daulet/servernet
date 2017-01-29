using Microsoft.WindowsAzure.Storage.Queue;

namespace Servernet.SelfHost.Azure.Queue
{
    public interface IQueueEntity
    {
        CloudQueueMessage GetCloudQueueMessage();
    }
}
