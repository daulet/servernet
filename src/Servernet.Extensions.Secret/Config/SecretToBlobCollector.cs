using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servernet.Extensions.Secret.Config
{
    internal class SecretToBlobCollector : IAsyncCollector<Secret>
    {
        private readonly SecretContainerAttribute _attribute;

        public SecretToBlobCollector(SecretContainerAttribute attribute)
        {
            _attribute = attribute;
        }

        public async Task AddAsync(Secret item, CancellationToken cancellationToken = new CancellationToken())
        {
            var storageAccount = CloudStorageAccount.Parse(_attribute.Connection);
            var storageClient = storageAccount.CreateCloudBlobClient();
            var secretContainer = storageClient.GetContainerReference("servernet-extensions-secret");
            await secretContainer.CreateIfNotExistsAsync(cancellationToken);

            var secret = item.Certificate.RawData;
            // @TODO decide block or page blobs
            var secretBlob = secretContainer.GetBlockBlobReference($"{_attribute.Container}/{item.Id}");
            await secretBlob.UploadFromByteArrayAsync(secret, 0, secret.Length, cancellationToken);
        }

        public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }
    }
}

