using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TableScanFunction.Model
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
