using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Servernet.Extensions.Secret.Config
{
    internal class StringToSecretConverter : IAsyncConverter<SecretAttribute, string>
    {
        public async Task<string> ConvertAsync(SecretAttribute attribute, CancellationToken cancellationToken)
        {
            var storageAccount = CloudStorageAccount.Parse(attribute.Connection);
            var storageClient = storageAccount.CreateCloudBlobClient();
            var secretBlob = await storageClient.GetBlobReferenceFromServerAsync(new Uri(attribute.SecretId), cancellationToken);

            using (var secretStream = new MemoryStream())
            {
                await secretBlob.DownloadToStreamAsync(secretStream, cancellationToken);
                return Encoding.UTF8.GetString(secretStream.ToArray());
            }
        }
    }
}
