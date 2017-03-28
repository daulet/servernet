using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.CLI
{
    internal class TypeSwitch
    {
        private readonly Dictionary<Type, Action<object>> _matches
            = new Dictionary<Type, Action<object>>();

        public TypeSwitch Case<T>(Action<T> action)
        {
            _matches.Add(typeof(T), (x) => action((T)x));
            return this;
        }

        public void Switch(object obj)
        {
            if (obj != null)
            {
                var objType = obj.GetType();
                if (_matches.ContainsKey(objType))
                {
                    _matches[objType](obj);
                }
            }
        }
    }
}
