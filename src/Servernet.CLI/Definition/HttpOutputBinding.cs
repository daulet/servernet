using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.CLI.Definition
{
    public class HttpOutputBinding : IBinding
    {
        public BindingType Type { get; } = BindingType.Http;
    }
}
