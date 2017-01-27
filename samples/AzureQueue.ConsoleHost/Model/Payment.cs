using Microsoft.WindowsAzure.Storage.Queue;
using Servernet.SelfHost.Azure.Queue;

namespace AzureQueue.ConsoleHost.Model
{
    public class Payment : IQueueEntity
    {
        private readonly CloudQueueMessage _cloudQueueMessage;

        public Payment(CloudQueueMessage cloudQueueMessage)
        {
            _cloudQueueMessage = cloudQueueMessage;
        }

        public CloudQueueMessage GetCloudQueueMessage()
        {
            return _cloudQueueMessage;
        }
    }
}
