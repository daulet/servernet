using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs;

namespace Servernet.Generator
{
    internal class FunctionLocator
    {
        private readonly IAssemblyLoader _assemblyLoader;
        private readonly ILogger _log;

        public FunctionLocator(IAssemblyLoader assemblyLoader, ILogger log)
        {
            _assemblyLoader = assemblyLoader;
            _log = log;
        }

        public IEnumerable<Tuple<Type, MethodInfo>> Locate(string assemblyPath)
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

            Type[] foundTypes;
            try
            {
                foundTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                throw new ArgumentException(
                    $"Failed to load types of assembly {assembly.FullName}", e);
            }

            var foundFunctions = new HashSet<Tuple<Type, MethodInfo>>();
            foreach (var type in foundTypes)
            {
                _log.Verbose($"Examining {type.Name}");
                try
                {
                    var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
                    foreach (var method in methods)
                    {
                        var attribute = method.GetCustomAttribute<FunctionNameAttribute>(inherit: false);
                        if (attribute != null)
                        {
                            _log.Verbose($"Found AzureFunction attribute on type {type.Name}, method {method.Name}");

                            foundFunctions.Add(new Tuple<Type, MethodInfo>(type, method));
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.Verbose($"Failed to enumerate methods of {type.Name}, ex: {e}");
                }
            }
            return foundFunctions;
        }
    }
}
