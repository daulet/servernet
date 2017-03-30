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

            var foundTypes = assembly.GetTypes();
            foreach (var type in foundTypes)
            {
                var attribute = type.GetCustomAttribute<AzureFunctionAttribute>(inherit: false);
                if (attribute != null)
                {
                    MethodInfo method;
                    try
                    {
                        method = type
                            .GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .SingleOrDefault();
                    }
                    catch (InvalidOperationException)
                    {
                        // @TODO add ability to specify [EntryPoint]
                        _log.Warning($"Multiple public static methods on {type.FullName} type");
                        continue;
                    }

                    yield return new Tuple<Type, MethodInfo>(type, method);
                }
            }
            yield break;
        }
    }
}
