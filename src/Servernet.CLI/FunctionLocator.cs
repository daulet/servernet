using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Servernet.CLI
{
    internal class FunctionLocator
    {
        private readonly ILogger _log;

        public FunctionLocator(ILogger log)
        {
            _log = log;
        }

        public IEnumerable<Tuple<Type, MethodInfo>> Locate(string assemblyPath)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(assemblyPath);
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
            
            foreach (var type in foundTypes)
            {
                _log.Verbose($"Examining {type.Name}");

                var attribute = type.GetCustomAttribute<AzureFunctionAttribute>(inherit: false);
                if (attribute != null)
                {
                    _log.Verbose($"Found AzureFunction attribute on {type.Name}");

                    MethodInfo method;
                    try
                    {
                        method = type
                            .GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .Single();
                    }
                    catch (InvalidOperationException)
                    {
                        // @TODO add ability to specify [EntryPoint]
                        _log.Warning($"Ambiguous or missing public static method on {type.FullName} type with AzureFunction attribute");
                        continue;
                    }

                    yield return new Tuple<Type, MethodInfo>(type, method);
                }
            }
        }
    }
}
