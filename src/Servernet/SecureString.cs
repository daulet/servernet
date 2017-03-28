using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet
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
