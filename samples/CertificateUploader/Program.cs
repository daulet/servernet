using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.WindowsAzure.Storage;
using Servernet.Secret;

namespace Servernet.Samples.CertificateUploader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["secretStorageAccount"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("certificates");
            blobContainer.CreateIfNotExists();

            var blob = blobContainer.GetBlockBlobReference("message_encrypting.pfx");
            var certificate = new X509Certificate2("message_encrypting.pfx", " ");
            var certificateToUpload = new Certificate(certificate);
            blob.UploadText(certificateToUpload.SerializedCertificateData);
        }
    }
}
