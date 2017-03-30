using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace Servernet.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                var assemblyPath = Path.Combine(Environment.CurrentDirectory, options.Assembly);
                
                LoadFunction(assemblyPath, options.Function, options.OutputDirectory);
            }
            else
            {
                Console.WriteLine("Invalid parameters");
            }
        }

        private static void LoadFunction(string assemblyPath, string fullFunctionName, string outputDirectory)
        {
            Assembly functionAssembly;
            try
            {
                functionAssembly = Assembly.LoadFrom(assemblyPath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Failed to load assembly at location: {assemblyPath}");
                return;
            }

            if (string.IsNullOrEmpty(fullFunctionName))
            {
                Console.WriteLine("Function name can't be empty");
                return;
            }
            // assuming function name can't contain a period
            var indexOfLastPeriod = fullFunctionName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
            if (indexOfLastPeriod <= 0)
            {
                Console.WriteLine($"Invalid fully qualified method name: {fullFunctionName}");
                return;
            }
            var typeName = fullFunctionName.Substring(0, indexOfLastPeriod);
            var functionName = fullFunctionName.Substring(indexOfLastPeriod + 1);

            Type functionType;
            try
            {
                functionType = functionAssembly.GetType(typeName, throwOnError: true, ignoreCase: true);
            }
            catch (TypeLoadException)
            {
                Console.WriteLine($"Failed to load type with name: {typeName}");
                return;
            }

            MethodInfo functionMethod;
            try
            {
                functionMethod = functionType.GetMethod(functionName, BindingFlags.Static | BindingFlags.Public);
            }
            catch (AmbiguousMatchException)
            {
                Console.WriteLine($"Failed to find unique method with name: {functionName}");
                return;
            }

            if (functionMethod == null)
            {
                Console.WriteLine($"Failed to find method with name: {functionName}");
                return;
            }

            ParameterInfo[] parameters;
            try
            {
                parameters = functionMethod.GetParameters();
            }
            catch (FileLoadException e)
            {
                Console.WriteLine($"Failed to load one of dependency assemblies for method '{functionMethod.Name}': {e}");
                return;
            }
            
            var functionBuilder = new FunctionBuilder(functionAssembly, functionType, functionMethod);
            
            foreach (var parameter in parameters)
            {
                object[] attributes;
                try
                {
                    attributes = parameter.GetCustomAttributes(inherit: false);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine($"Failed to load one of dependency assemblies for argument '{parameter.Name}': {e}");
                    return;
                }
                
                foreach (var attribute in attributes)
                {
                    // @TODO can't allow two bindings, but can have multiple custom attributes
                    functionBuilder.AddBinding(parameter, attribute);
                }
            }

            if (string.IsNullOrEmpty(outputDirectory))
            {
                outputDirectory = functionType.Name;
            }

            functionBuilder.Validate(new ColorfulConsole());

            Directory.CreateDirectory(outputDirectory);

            // @TODO also add project.json and include Servernet as nuget package
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-csharp#package-management
            using (var bindingFile = new StreamWriter($"{outputDirectory}/function.json"))
            {
                bindingFile.Write(functionBuilder.ToString());
            }

            // List of dependencies available to any function and hence should not be part of release
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-csharp
            var excludedAssemblies = new HashSet<string>()
            {
                // Microsoft.Azure.WebJobs and its dependencies
                "Microsoft.Azure.WebJobs.dll",
                "Microsoft.Azure.WebJobs.Host.dll",
                "Microsoft.WindowsAzure.Storage.dll",
                "Newtonsoft.Json.dll",
                // Microsoft.Azure.WebJobs.Extensions and its dependencies
                "Microsoft.Azure.WebJobs.Extensions.dll",
                "Microsoft.Azure.WebJobs.dll",
                "NCrontab.dll",
                "System.Threading.Tasks.Dataflow.dll",
                // Microsoft.WindowsAzure.Storage and its dependencies
                "Microsoft.WindowsAzure.Storage.dll",
                "Microsoft.Azure.KeyVault.Core.dll",
                "Microsoft.Data.Edm.dll",
                "Microsoft.Data.OData.dll",
                "Microsoft.Data.Services.Client.dll",
                "Newtonsoft.Json.dll",
                "System.Spatial.dll",
                // Newtonsoft.Json and its dependencies
                "Newtonsoft.Json.dll",
            };

            var directory = new FileInfo(functionAssembly.Location).Directory;
            var allReferencedAssemblies = directory.GetFiles();
            foreach (var referencedAssembly in allReferencedAssemblies)
            {
                if (excludedAssemblies.Contains(referencedAssembly.Name))
                {
                    // don't copy assemblies that are available to Azure Functions anyway
                    continue;
                }
                File.Copy(referencedAssembly.FullName, Path.Combine(outputDirectory, referencedAssembly.Name), overwrite: true);
            }
        }
    }
}
