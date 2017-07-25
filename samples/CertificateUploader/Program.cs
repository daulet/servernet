using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Servernet.Secret;

namespace Servernet.Samples.CertificateUploader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var certificate = new X509Certificate2("message_encrypting.pfx", " ");
            UploadAsync("certificates", "message_encrypting.pfx", certificate).Wait();
        }

        private static async Task UploadAsync(string container, string id, X509Certificate2 certificate)
        {
            using (var client = new HttpClient())
            {
                var content = new ByteArrayContent(certificate.RawData);
                var response = await client.PostAsync($"http://localhost:7071/api/{container}/{id}", content);
                await response.Content.ReadAsStringAsync();
            }
        }
    }
}
