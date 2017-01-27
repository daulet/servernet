using DistributedTableProcessor.Model;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet.SelfHost.Azure.Table;
using System;

namespace DistributedTableProcessor.Storage
{
    public class CustomerTableEntityFactory : ITableEntityFactory<CustomerTable>
    {
        public IsStoredIn<CustomerTable> CreateEntity(DynamicTableEntity tableEntity)
        {
            EntityProperty entityProperty;
            if (tableEntity.Properties.TryGetValue("type", out entityProperty))
            {
                switch (entityProperty.StringValue)
                {
                    case "customer":
                        return new Customer(tableEntity);
                    case "purchase":
                        return new Purchase(tableEntity);
                    default:
                        throw new ArgumentException($"Unexpected type {entityProperty.StringValue}");
                }
            }
            else
            {
                throw new ArgumentException($"Unable to parse entity {tableEntity.PartitionKey}/{tableEntity.RowKey}");
            }
        }
    }
}
