using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.Azure
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class TableInputAttribute : Attribute
    {
        private readonly string _tableName;

        public TableInputAttribute(string tableName)
        {
            _tableName = tableName;
        }
    }
}
