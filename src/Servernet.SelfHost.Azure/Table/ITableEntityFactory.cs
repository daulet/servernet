using Microsoft.WindowsAzure.Storage.Table;

namespace Servernet.SelfHost.Azure.Table
{
    public interface ITableEntityFactory<TTableDefinition>
        where TTableDefinition : ITableDefinition
    {
        IsStoredIn<TTableDefinition> CreateEntity(DynamicTableEntity tableEntity);
    }
}
