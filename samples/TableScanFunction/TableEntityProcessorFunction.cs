﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet;
using TableScanFunction.Model;

namespace TableScanFunction
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
