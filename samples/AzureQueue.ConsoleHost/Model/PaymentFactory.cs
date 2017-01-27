using Microsoft.WindowsAzure.Storage.Queue;
using Servernet.SelfHost.Azure.Queue;

namespace AzureQueue.ConsoleHost.Model
{
    public class PaymentFactory : IQueueItemFactory<PaymentQueue>
    {
        public IsStoredIn<PaymentQueue> CreateQueueItem(CloudQueueMessage message)
        {
            return new Payment(message);
        }
    }
}
