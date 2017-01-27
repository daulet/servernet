using Microsoft.WindowsAzure.Storage;
using Servernet.SelfHost.Azure.Table;

namespace DistributedTableProcessor.Storage
{
    public class CustomerTable : ITableDefinition
    {
        public CloudStorageAccount Account { get; } = CloudStorageAccount.DevelopmentStorageAccount;

        public string Name { get; } = "Customers";
    }
}
