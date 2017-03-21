using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Servernet;
using TableScanFunction.Model;

namespace TableScanFunction
{
    public class PaginationFunction : IFunction
    {
        private readonly TableSegment _tableSegment;

        public PaginationFunction(TableSegment tableSegment)
        {
            _tableSegment = tableSegment;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
