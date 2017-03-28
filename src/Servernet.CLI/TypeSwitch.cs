using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Servernet.CLI
{
    internal class TypeSwitch
    {
        private readonly Dictionary<Type, Action<ParameterInfo, object>> _matches
            = new Dictionary<Type, Action<ParameterInfo, object>>();

        public TypeSwitch Case<T>(Action<ParameterInfo, T> action)
        {
            _matches.Add(typeof(T), (x, y) => action(x, (T)y));
            return this;
        }

        public void Switch(ParameterInfo parameter, object obj)
        {
            if (obj != null)
            {
                var objType = obj.GetType();
                if (_matches.ContainsKey(objType))
                {
                    _matches[objType](parameter, obj);
                }
            }
        }
    }
}
