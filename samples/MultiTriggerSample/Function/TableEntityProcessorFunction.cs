using Microsoft.WindowsAzure.Storage.Table;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Function
{
    public class TableEntityProcessorFunction : IFunction<DynamicTableEntity, bool>
    {
        private readonly IFunction<Transaction, bool> _transactionProcessor;

        public TableEntityProcessorFunction(
            IFunction<Transaction, bool> transactionProcessor)
        {
            _transactionProcessor = transactionProcessor;
        }

        public bool Run(DynamicTableEntity input)
        {
            var transaction = new Transaction(input);
            return _transactionProcessor.Run(transaction);
        }
    }
}
