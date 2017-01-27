using Microsoft.WindowsAzure.Storage;

namespace Servernet.SelfHost.Azure.Table
{
    public interface ITableDefinition
    {
        CloudStorageAccount Account { get; }

        string Name { get; }
    }
}
