namespace Servernet.Secret
{
    public class SecureString
    {
        public SecureString(string secretString)
        {
            StringValue = secretString;
        }

        public string StringValue { get; set; }
    }
}
