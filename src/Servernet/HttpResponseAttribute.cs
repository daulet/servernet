using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpResponseAttribute : Attribute
    {
    }
}
