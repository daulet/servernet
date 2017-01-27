using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Servernet.SelfHost.Azure.Queue;

namespace AzureQueue.ConsoleHost.Model
{
    public class PaymentQueue : IQueueDefinition<Payment>
    {
        public CloudStorageAccount Account { get; } = CloudStorageAccount.DevelopmentStorageAccount;

        public string Name { get; } = "PaymentQueue";

        public Payment GetEntity(CloudQueueMessage cloudQueueMessage)
        {
            return new Payment(cloudQueueMessage);
        }
    }
}
