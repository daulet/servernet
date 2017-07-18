using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Servernet.Generator.Definition;

namespace Servernet.Generator
{
    internal class ReleaseBuilder
    {
        private readonly IFileSystem _fileSystem;

        public ReleaseBuilder(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Release(string sourceDirectoryPath, string targetDirectoryPath, Function function)
        {
            _fileSystem.CreateDirectory(targetDirectoryPath);

            // @TODO also add project.json and include Servernet as nuget package
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-csharp#package-management
            using (var bindingFile = _fileSystem.CreateFileWriter($"{targetDirectoryPath}/function.json"))
            {
                var serializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                };
                serializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true));
                bindingFile.Write(
                    JsonConvert.SerializeObject(function, Formatting.Indented, serializerSettings));
            }
            
            var allReferencedAssemblies = _fileSystem.GetFiles(sourceDirectoryPath);
            foreach (var referencedAssembly in allReferencedAssemblies)
            {
                if (KnownAssemblies.Contains(referencedAssembly.Name))
                {
                    // don't copy assemblies that are available to Azure Functions anyway
                    continue;
                }
                _fileSystem.CopyFile(referencedAssembly.FullName, Path.Combine(targetDirectoryPath, referencedAssembly.Name), overwrite: true);
            }
        }

        // List of dependencies available to any function and hence should not be part of release
        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-csharp
        static readonly HashSet<string> KnownAssemblies = new HashSet<string>()
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
            // The following assemblies are automatically added by the Azure Functions hosting environment
            // (https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-csharp#referencing-external-assemblies):
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "System.Xml.dll",
            "System.Net.Http",
            "System.Net.Http.Formatting.dll",
            "System.Web.Http.dll",
        };
    }
}
