using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet;

namespace TableScanFunction.Model
{
    public class TableSegment
    {
        public TableContinuationToken ContinuationToken { get; set; }
    }
}
