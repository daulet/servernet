using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class HttpTrigger : Attribute
    {
        // @TODO route as constructor parameter
        // @TODO HttpRoute attribute to dynamically parse and assign HttpTrigger route parameters
        // @TODO decide how to define accepted HTTP method
    }
}
