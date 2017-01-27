using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servernet.SelfHost.Azure.Table
{
    public class TableSource<TTableDefinition> : IInputSource<IsStoredIn<TTableDefinition>>
        where TTableDefinition : ITableDefinition
    {
        private readonly Lazy<CloudTable> _cloudTable;
        private readonly ITableEntityFactory<TTableDefinition> _factory;
        private readonly TableQuery _query;

        public TableSource(TTableDefinition tableDefinition, TableQuery query, ITableEntityFactory<TTableDefinition> factory)
        {
            _factory = factory;
            _query = query;
            _cloudTable = new Lazy<CloudTable>(() =>
            {
                var tableClient = tableDefinition.Account.CreateCloudTableClient();
                return tableClient.GetTableReference(tableDefinition.Name);
            });
        }

        public IEnumerable<IsStoredIn<TTableDefinition>> GetEnumerable()
        {
            return _cloudTable.Value.ExecuteQuery(_query).Select(x => _factory.CreateEntity(x));
        }

        public void MarkAsDone(IsStoredIn<TTableDefinition> input)
        {
            // unlike Azure Queue messages which meant to be deleted after processing,
            // Azure Table entities don't necessarily have to be updated or deleted,
            // and in cases in which they do it should be implemented by corresponding IAction.
        }
    }
}
