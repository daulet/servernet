using System;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI.Definition
{
    public class TableOutputBinding : IBinding
    {
        internal TableOutputBinding(Type functionType, string paramName, TableAttribute attribute)
        {
            Connection = $"{functionType.Name}_output_table_{paramName}";
            Name = paramName;
            PartitionKey = attribute.PartitionKey;
            RowKey = attribute.RowKey;
            TableName = attribute.TableName;
        }

        public string Connection { get; }

        public BindingDirection Direction { get; } = BindingDirection.Out;

        public string Name { get; }

        public string PartitionKey { get; }

        public string RowKey { get; }

        public string TableName { get; }

        public BindingType Type { get; } = BindingType.Table;
    }
}
