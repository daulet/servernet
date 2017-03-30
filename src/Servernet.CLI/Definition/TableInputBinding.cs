using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class TableInputBinding : IBinding
    {
        public TableInputBinding(Type functionType, string paramName, TableAttribute attribute)
        {
            Connection = $"{functionType.Name}_input_table_{paramName}";
            Direction = "in";
            Filter = attribute.Filter;
            Name = paramName;
            PartitionKey = attribute.PartitionKey;
            RowKey = attribute.RowKey;
            // @TODO enforce table name rules:
            // https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/Understanding-the-Table-Service-Data-Model
            TableName = attribute.TableName;
            Take = attribute.Take;
        }

        public string Connection { get; set; }

        public string Direction { get; set; }

        public string Filter { get; set; }

        public string Name { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string TableName { get; set; }

        public int Take { get; set; }

        public BindingType Type { get; set; } = BindingType.Table;
    }
}
