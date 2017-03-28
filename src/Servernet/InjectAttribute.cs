using System;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class InjectAttribute : Attribute
    {
        private readonly Type _typeToRegister;

        /// <summary>
        /// Used for concrete types that don't require any extra registrations
        /// </summary>
        public InjectAttribute()
        { }

        /// <summary>
        /// Used for types that require resolving dependency instance somewhere in dependency tree
        /// </summary>
        /// <param name="typeToRegister"></param>
        public InjectAttribute(Type typeToRegister)
        {
            _typeToRegister = typeToRegister;
        }
    }
}
