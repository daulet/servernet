using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Servernet.SelfHost.Azure.Queue
{
    public class QueueSource<TEntity> : IInputSource<TEntity>
        where TEntity : IQueueEntity
    {
        private readonly Lazy<CloudQueue> _cloudQueue;
        private readonly IQueueDefinition<TEntity> _queueDefinition;
        private readonly TimeSpan _timeout;

        public QueueSource(IQueueDefinition<TEntity> queueDefinition, TimeSpan timeout)
        {
            _queueDefinition = queueDefinition;
            _timeout = timeout;
            _cloudQueue = new Lazy<CloudQueue>(() =>
            {
                var queueClient = queueDefinition.Account.CreateCloudQueueClient();
                return queueClient.GetQueueReference(queueDefinition.Name);
            },
            LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IEnumerable<TEntity> GetEnumerable()
        {
            return EnumerateUntilNull(() =>
            {
                var queueMessage = _cloudQueue.Value.GetMessage(_timeout);
                return _queueDefinition.GetEntity(queueMessage);
            });
        }

        public void MarkAsDone(TEntity input)
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
