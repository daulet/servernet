﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableScanFunction.Model
{
    public class TableEntityId
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }
    }
}