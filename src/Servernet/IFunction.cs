using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet
{
    public interface IFunction<in TInput, out TOutput>
    {
        TOutput Run(TInput input);
    }
}
