using Microsoft.WindowsAzure.Storage.Table;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Function
{
    public class TableEntityProcessorFunction : IFunction<DynamicTableEntity, bool>
    {
        private readonly IFunction<Transaction, bool> _transactionFunction;

        public TableEntityProcessorFunction(IFunction<Transaction, bool> transactionFunction)
        {
            _transactionFunction = transactionFunction;
        }

        public bool Run(DynamicTableEntity input)
        {
            var transaction = new Transaction(input);
            return _transactionFunction.Run(transaction);
        }
    }
}
