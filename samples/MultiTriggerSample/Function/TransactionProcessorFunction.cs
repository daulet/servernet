using System;
using Servernet.Samples.MultiTriggerSample.Model;

namespace Servernet.Samples.MultiTriggerSample.Function
{
    public class TransactionProcessorFunction : IFunction<Transaction, bool>
    {
        public bool Run(Transaction input)
        {
            throw new NotImplementedException();
        }
    }
}
