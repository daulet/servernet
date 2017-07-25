using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servernet.Extensions.Secret.Config
{
    internal class BlobToCertificateConverter : IAsyncConverter<SecretAttribute, X509Certificate2>
    {
        public async Task<X509Certificate2> ConvertAsync(SecretAttribute attribute, CancellationToken cancellationToken)
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
                    var rawData = secretStream.ToArray();
                    return new X509Certificate2(rawData);
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
