using DistributedTableProcessor.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet.SelfHost.Azure.Table;

namespace DistributedTableProcessor.Model
{
    public class Purchase : IsStoredIn<CustomerTable>
    {
        private readonly DynamicTableEntity _tableEntity;

        public Purchase(DynamicTableEntity tableEntity)
        {
            _tableEntity = tableEntity;
        }
    }
}
