using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.Generator.Definition
{
    public class TableInputBinding : IBinding
    {
        internal TableInputBinding(Type functionType, string paramName, TableAttribute attribute)
        {
            Connection = $"{functionType.Name}_input_table_{paramName}";
            Filter = attribute.Filter;
            Name = paramName;
            PartitionKey = attribute.PartitionKey;
            RowKey = attribute.RowKey;
            // @TODO enforce table name rules:
            // https://docs.microsoft.com/en-us/rest/api/storageservices/fileservices/Understanding-the-Table-Service-Data-Model
            TableName = attribute.TableName;
            Take = attribute.Take;
        }

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.In;

        public string Filter { get; }

        public string Name { get; }

        public string PartitionKey { get; }

        public string RowKey { get; }

        public string TableName { get; }

        public int Take { get; }

        public BindingType Type { get; } = BindingType.Table;
    }
}
