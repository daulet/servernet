using System;
using System.Collections.Generic;

namespace Servernet.CLI
{
    internal class TypeSwitch<TParam>
    {
        private readonly Dictionary<Type, Action<TParam, object>> _matches
            = new Dictionary<Type, Action<TParam, object>>();

        public TypeSwitch<TParam> Case<T>(Action<TParam, T> action)
        {
            _matches.Add(typeof(T), (x, y) => action(x, (T)y));
            return this;
        }

        public void Switch(TParam parameter, object obj)
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
