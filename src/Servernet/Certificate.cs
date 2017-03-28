using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Servernet
{
    /// <summary>
    /// Used to store X509Certificate2 certificates in blob storage
    /// and bindable to Azure Function
    /// </summary>
    public class Certificate
    {
        public Certificate(X509Certificate2 certificate)
        {
            X509Certificate = certificate;
        }

        [JsonIgnore]
        public X509Certificate2 X509Certificate { get; private set; }

        // @TODO hide setters when we can wrap Azure Function input parameters
        public byte[] RawCertificateData
        {
            get
            {
                return X509Certificate.RawData;
            }
            set
            {
                X509Certificate = new X509Certificate2(value);
            }
        }
    }
}
