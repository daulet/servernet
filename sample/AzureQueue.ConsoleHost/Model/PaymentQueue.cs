using Microsoft.WindowsAzure.Storage;
using Servernet.SelfHost.Azure.Queue;

namespace AzureQueue.ConsoleHost.Model
{
    public class PaymentQueue : IQueueDefinition
    {
        public CloudStorageAccount Account { get; } = CloudStorageAccount.DevelopmentStorageAccount;

        public string Name { get; } = "PaymentQueue";
    }
}
