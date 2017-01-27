using Microsoft.WindowsAzure.Storage.Queue;
using Servernet.SelfHost.Azure.Queue;

namespace DistributedTableProcessor.Model
{
    public class TablePage : IQueueEntity
    {
        // TODO store both query and continuation so we don't have to assume continuation format

        public string TableQuery { get; }

        #region Boilerplate

        private readonly CloudQueueMessage _message;

        public TablePage(CloudQueueMessage message)
        {
            _message = message;
            TableQuery = _message.AsString;
        }

        public CloudQueueMessage GetCloudQueueMessage()
        {
            return _message;
        }

        #endregion
    }
}
