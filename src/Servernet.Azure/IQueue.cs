using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.Azure
{
    public interface IQueue<in T>
    {
        void AddMessage(T message);
    }
}
