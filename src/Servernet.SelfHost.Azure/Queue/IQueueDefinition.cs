using Microsoft.WindowsAzure.Storage;

namespace Servernet.SelfHost.Azure.Queue
{
    public interface IQueueDefinition
    {
        CloudStorageAccount Account { get; }

        string Name { get; }
    }
}
