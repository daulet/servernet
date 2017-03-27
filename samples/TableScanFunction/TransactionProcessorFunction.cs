using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servernet;
using TableScanFunction.Model;

namespace TableScanFunction
{
    public class TransactionProcessorFunction : IFunction<Transaction, bool>
    {
        public bool Run(Transaction input)
        {
            throw new NotImplementedException();
        }
    }
}
