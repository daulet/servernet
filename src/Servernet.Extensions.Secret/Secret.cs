using System.Security.Cryptography.X509Certificates;

namespace Servernet.Extensions.Secret
{
    public class Secret
    {
        public X509Certificate2 Certificate { get; set; }

        public string Id { get; set; }
    }
}
