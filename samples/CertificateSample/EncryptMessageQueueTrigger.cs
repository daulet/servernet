﻿using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Servernet.Samples.CertificateSample
{
    public class EncryptMessageQueueTrigger
    {
        public static void Run(
            [QueueTrigger("encrypt-queue")] string message,
            [Secret("certificates/message_encrypting.cer")] Certificate certificate,
            TraceWriter traceWriter)
        {
            var byteConverter = new ASCIIEncoding();
            var encodedBytes = byteConverter.GetBytes(message);;

            var cryptoServiceProvider = (RSACryptoServiceProvider)certificate.X509Certificate.PublicKey.Key;
            var encryptedBytes = cryptoServiceProvider.Encrypt(encodedBytes, false);

            traceWriter.Info($"Encrypted value for '{message}' is {Convert.ToBase64String(encryptedBytes)}");
        }
    }
}
