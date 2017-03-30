using System;
using System.IO;
using System.Reflection;

namespace Servernet.CLI
{
    internal class FunctionLocator
    {
        public Tuple<Type, MethodInfo> Locate(string assemblyPath, string typeName, string methodName)
        {
            Assembly functionAssembly;
            try
            {
                functionAssembly = Assembly.LoadFrom(assemblyPath);
            }
            catch (FileNotFoundException e)
            {
                throw new ArgumentException(
                    $"Failed to load assembly at location: {assemblyPath}", e);
            }

            Type functionType;
            try
            {
                functionType = functionAssembly.GetType(typeName, throwOnError: true, ignoreCase: true);
            }
            catch (TypeLoadException e)
            {
                throw new ArgumentException(
                    $"Failed to load type with name: {typeName}", e);
            }

            MethodInfo functionMethod;
            try
            {
                functionMethod = functionType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            }
            catch (AmbiguousMatchException e)
            {
                throw new ArgumentException(
                    $"Failed to find unique method with name: {methodName}", e);
            }

            if (functionMethod == null)
            {
                throw new ArgumentException(
                    $"Failed to find method with name: {methodName}");
            }

            return new Tuple<Type, MethodInfo>(functionType, functionMethod);
        }
    }
}
