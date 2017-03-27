using Microsoft.WindowsAzure.Storage.Table;

namespace Servernet.Samples.MultiTriggerSample.Model
{
    public class Transaction
    {
        private readonly DynamicTableEntity _tableEntity;

        public Transaction(DynamicTableEntity tableEntity)
        {
            _tableEntity = tableEntity;
        }

        public double Amount { get; set; }

        public string CustomerName { get; set; }

    }
}
