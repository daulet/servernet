using System;
using System.Collections.Generic;
using System.IO;

namespace Servernet.CLI
{
    internal partial class Program
    {
        private readonly ILogger _log;
        private readonly Options _options;

        private Program(ILogger log, Options options)
        {
            _log = log;
            _options = options;
        }

        private void Run()
        {
            var assemblyPath = Path.Combine(Environment.CurrentDirectory, _options.Assembly);
            var fullFunctionName = _options.Function;
            var outputDirectory = _options.OutputDirectory;

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
            var methodName = fullFunctionName.Substring(indexOfLastPeriod + 1);

            var locator = new MethodLocator();
            var locatedMethod = locator.Locate(assemblyPath, typeName, methodName);
            var functionType = locatedMethod.Item1;
            var functionMethod = locatedMethod.Item2;

            var parser = new AttributeParser();
            var functionBuilder = parser.ParseEntryPoint(functionType, functionMethod);

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

            var directory = new FileInfo(functionType.Assembly.Location).Directory;
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
