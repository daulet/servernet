using AzureTable.ConsoleHost.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet.SelfHost.Azure.Table;

namespace AzureTable.ConsoleHost.Model
{
    public class Customer : IsStoredIn<CustomerTable>
    {
        private readonly DynamicTableEntity _tableEntity;

        public Customer(DynamicTableEntity tableEntity)
        {
            _tableEntity = tableEntity;
        }
    }
}
