using System;
using System.Security.Cryptography.X509Certificates;

namespace Servernet.Secret
{
    /// <summary>
    /// Used to store X509Certificate2 certificates in blob storage
    /// and bindable to Azure Function
    /// </summary>
    /// <remarks>
    /// Should really be a POCO, but blocked by Azure Functions bug:
    /// https://github.com/Azure/azure-webjobs-sdk-script/issues/869
    /// </remarks>
    public class Certificate
    {
        public Certificate(string certificateData)
        {
            X509Certificate = new X509Certificate2(Convert.FromBase64String(certificateData));
            SerializedCertificateData = certificateData;
        }

        public Certificate(X509Certificate2 certificate)
        {
            X509Certificate = certificate;
            SerializedCertificateData = Convert.ToBase64String(certificate.RawData);
        }

        public string SerializedCertificateData { get; private set; }

        public X509Certificate2 X509Certificate { get; private set; }
    }
}
