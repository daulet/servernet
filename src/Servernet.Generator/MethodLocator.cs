using System;
using System.IO;
using System.Reflection;

namespace Servernet.Generator
{
    internal class MethodLocator
    {
        private readonly IAssemblyLoader _assemblyLoader;

        public MethodLocator(IAssemblyLoader assemblyLoader)
        {
            _assemblyLoader = assemblyLoader;
        }

        public Tuple<Type, MethodInfo> Locate(string assemblyPath, string typeName, string methodName)
        {
            Assembly assembly;
            try
            {
                assembly = _assemblyLoader.LoadFrom(assemblyPath);
            }
            catch (FileNotFoundException e)
            {
                throw new ArgumentException(
                    $"Failed to load assembly at location: {assemblyPath}", e);
            }

            Type type;
            try
            {
                type = assembly.GetType(typeName, throwOnError: true, ignoreCase: true);
            }
            catch (TypeLoadException e)
            {
                throw new ArgumentException(
                    $"Failed to load type with name: {typeName}", e);
            }

            MethodInfo method;
            try
            {
                method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            }
            catch (AmbiguousMatchException e)
            {
                throw new ArgumentException(
                    $"Failed to find unique method with name: {methodName}", e);
            }

            if (method == null)
            {
                throw new ArgumentException(
                    $"Failed to find method with name: {methodName}");
            }

            return new Tuple<Type, MethodInfo>(type, method);
        }
    }
}
