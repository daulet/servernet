using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class TableOutputBinding : IBinding
    {
        public TableOutputBinding(Type functionType, string paramName, TableAttribute attribute)
        {
            Connection = $"{functionType.Name}_output_table_{paramName}";
            Direction = "out";
            Name = paramName;
            PartitionKey = attribute.PartitionKey;
            RowKey = attribute.RowKey;
            TableName = attribute.TableName;
            Type = "table";
        }

        public string Connection { get; set; }
        public string Direction { get; set; }
        public string Name { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string TableName { get; set; }
        public string Type { get; set; }
    }
}
