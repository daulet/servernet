using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet
{
    public enum HttpAuthLevel
    {
        /// <summary>
        /// No API key is required.
        /// </summary>
        Anonymous,
        /// <summary>
        /// A function-specific API key is required. This is the default value if none is provided.
        /// </summary>
        Function,
        /// <summary>
        /// The master key is required.
        /// </summary>
        Admin,
    }
}
