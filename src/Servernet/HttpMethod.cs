using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet
{
    [Flags]
    public enum HttpMethod
    {
        Delete = 1 << 0,
        Get = 1 << 1,
        Post = 1 << 2,
        Patch = 1 << 3,
        Put = 1 << 4,
    }
}
