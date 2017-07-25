using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servernet.Extensions.Secret.Config
{
    internal class BlobToCertificateConverter : IAsyncConverter<SecretAttribute, string>
    {
        public async Task<string> ConvertAsync(SecretAttribute attribute, CancellationToken cancellationToken)
        {
            var storageAccount = CloudStorageAccount.Parse(attribute.Connection);
            var storageClient = storageAccount.CreateCloudBlobClient();
            var secretContainer = storageClient.GetContainerReference("servernet-extensions-secret");

            try
            {
                var secretBlob = await secretContainer.GetBlobReferenceFromServerAsync(
                    $"{attribute.Container}/{attribute.Id}",
                    cancellationToken);

                using (var secretStream = new MemoryStream())
                {
                    await secretBlob.DownloadToStreamAsync(secretStream, cancellationToken);
                    return Encoding.UTF8.GetString(secretStream.ToArray());
                }
            }
            catch (StorageException ex)
                when (ex.RequestInformation.HttpStatusCode
                    == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
