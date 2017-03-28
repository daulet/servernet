using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.CertificateSample
{
    public class EncryptMessageQueueTrigger
    {
        public static void Run(
            [QueueTrigger("encrypt-queue")] string message,
            [Secret("certificates/message_encrypting.pfx")] string certificateData,
            TraceWriter traceWriter)
        {
            var certificate = new Certificate(certificateData);

            var byteConverter = new ASCIIEncoding();
            var encodedBytes = byteConverter.GetBytes(message);;

            var cryptoServiceProvider = (RSACryptoServiceProvider)certificate.X509Certificate.PublicKey.Key;
            var encryptedBytes = cryptoServiceProvider.Encrypt(encodedBytes, false);

            traceWriter.Info($"Using certificate issued by {certificate.X509Certificate.IssuerName.Name} to encrypt");
            traceWriter.Info($"Encrypted value for '{message}' is {Convert.ToBase64String(encryptedBytes)}");
        }
    }
}
