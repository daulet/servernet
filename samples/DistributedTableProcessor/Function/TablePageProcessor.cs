using DistributedTableProcessor.Model;
using Microsoft.WindowsAzure.Storage.Table;
using Servernet;
using Servernet.SelfHost;
using Servernet.SelfHost.Azure.Table;
using System;
using System.Linq;
using System.Threading;

namespace DistributedTableProcessor.Function
{
    public class TablePageProcessor<TTableDefinition> : IAction<TablePage>
        where TTableDefinition : ITableDefinition
    {
        private readonly IAction<IsStoredIn<TTableDefinition>> _entityProcessor;
        private readonly ITableEntityFactory<TTableDefinition> _factory;
        private readonly Lazy<CloudTable> _cloudTable;

        public TablePageProcessor(
            TTableDefinition tableDefinition,
            ITableEntityFactory<TTableDefinition> factory,
            IAction<IsStoredIn<TTableDefinition>> entityProcessor)
        {
            _factory = factory;
            _entityProcessor = entityProcessor;

            _cloudTable = new Lazy<CloudTable>(() =>
            {
                var tableClient = tableDefinition.Account.CreateCloudTableClient();
                return tableClient.GetTableReference(tableDefinition.Name);
            },
            LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public void Run(TablePage input)
        {
            var tableSegment = _cloudTable.Value.ExecuteQuerySegmented(
                new TableQuery().Where(input.TableQuery),
                token: null);

            if (tableSegment.ContinuationToken != null)
            {
                var nextTableSegment = tableSegment.ContinuationToken;
                // TODO insert back into the queue
            }

            var source = new ArraySource<IsStoredIn<TTableDefinition>>(tableSegment
                .Results
                .Select(x => _factory.CreateEntity(x))
                .ToArray());
            var runner = new Runner<IsStoredIn<TTableDefinition>>(source, _entityProcessor);

            runner.Run();
        }
    }
}
