using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Servernet.SelfHost.Azure.Queue
{
    public class QueueSource<TQueueDefinition> : IInputSource<IsStoredIn<TQueueDefinition>>
        where TQueueDefinition : IQueueDefinition
    {
        private readonly Lazy<CloudQueue> _cloudQueue;
        private readonly IQueueItemFactory<TQueueDefinition> _factory;
        private readonly TimeSpan _timeout;

        public QueueSource(IQueueItemFactory<TQueueDefinition> factory, TQueueDefinition queueDefinition, TimeSpan timeout)
        {
            _factory = factory;
            _timeout = timeout;
            _cloudQueue = new Lazy<CloudQueue>(() =>
            {
                var queueClient = queueDefinition.Account.CreateCloudQueueClient();
                return queueClient.GetQueueReference(queueDefinition.Name);
            },
            LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IEnumerable<IsStoredIn<TQueueDefinition>> GetEnumerable()
        {
            return EnumerateUntilNull(() =>
            {
                var queueMessage = _cloudQueue.Value.GetMessage(_timeout);
                return _factory.CreateQueueItem(queueMessage);
            });
        }

        public void MarkAsDone(IsStoredIn<TQueueDefinition> input)
        {
            _cloudQueue.Value.DeleteMessage(input.GetCloudQueueMessage());
        }

        private static IEnumerable<T> EnumerateUntilNull<T>(Func<T> producer)
        {
            T input;
            while ((input = producer()) != null)
            {
                yield return input;
            }
        }
    }
}
