using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Servernet.Generator.Definition;

namespace Servernet.Generator
{
    internal class ReleaseBuilder
    {
        private readonly IFileSystem _fileSystem;
        private readonly Lazy<ImmutableHashSet<string>> _knownAssemblies;

        public ReleaseBuilder(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;

            _knownAssemblies = new Lazy<ImmutableHashSet<string>>(() =>
            {
                var assemblies = new HashSet<string>();
                var seen = new HashSet<Package>();
                var unvisited = new Stack<Package>();
                unvisited.Push(RootPackage);
                while (unvisited.Count > 0)
                {
                    var package = unvisited.Pop();
                    seen.Add(package);

                    assemblies.UnionWith(package.Assemblies);

                    foreach (var dependency in package.Dependencies.Except(seen))
                    {
                        unvisited.Push(dependency);
                    }
                }
                return assemblies.ToImmutableHashSet();
            });
        }

        public void Release(string sourceDirectoryPath, string targetDirectoryPath, Function function)
        {
            _fileSystem.CreateDirectory(targetDirectoryPath);

            // @TODO also add project.json and include Servernet as nuget package
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-csharp#package-management
            
            var serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            serializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true));
            var functionDefinition = JsonConvert.SerializeObject(function, Formatting.Indented, serializerSettings);
            _fileSystem.WriteToFile($"{targetDirectoryPath}/function.json", functionDefinition);
            
            var allReferencedAssemblies = _fileSystem.GetFiles(sourceDirectoryPath);
            foreach (var referencedAssembly in allReferencedAssemblies
                .Where(x => !_knownAssemblies.Value.Contains(x.Name)))
            {
                _fileSystem.CopyFile(referencedAssembly.FullName, Path.Combine(targetDirectoryPath, referencedAssembly.Name), overwrite: true);
            }
        }

        // https://www.nuget.org/packages/Newtonsoft.Json/9.0.1
        private static readonly Package NewtonsoftJson = new Package("Newtonsoft.Json",
            new HashSet<string>()
            {
                "Newtonsoft.Json.dll",
            });

        // https://www.nuget.org/packages/Microsoft.Azure.WebJobs/2.1.0-beta1
        private static readonly Package WebJobs = new Package("Microsoft.Azure.WebJobs",
            new HashSet<string>()
            {
                "Microsoft.Azure.WebJobs.dll",
            },
            new HashSet<Package>()
            {
                // https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Core/2.1.0-beta1
                new Package("Microsoft.Azure.WebJobs.Core",
                    new HashSet<string>()
                    {
                        "Microsoft.Azure.WebJobs.Host.dll",
                    }),

                // https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/1.1.1
                new Package("Microsoft.Extensions.Logging.Abstractions",
                    new HashSet<string>()
                    {
                        "Microsoft.Extensions.Logging.Abstractions.dll",
                    },
                    new HashSet<Package>()
                    {
                        // https://www.nuget.org/packages/NETStandard.Library/2.0.0-preview2-25401-01
                        new Package("Flat.List.Of.Logging.Dependencies",
                            new HashSet<string>()
                            {
                                "Microsoft.Win32.Primitives.dll",
                                "System.AppContext.dll",
                                "System.Console.dll",
                                "System.Globalization.Calendars.dll",
                                "System.IO.Compression.dll",
                                "System.IO.Compression.ZipFile.dll",
                                "System.IO.FileSystem.dll",
                                "System.IO.FileSystem.Primitives.dll",
                                "System.Net.Http.dll",
                                "System.Net.Sockets.dll",
                                "System.Runtime.InteropServices.RuntimeInformation.dll",
                                "System.Security.Cryptography.Algorithms.dll",
                                "System.Security.Cryptography.Encoding.dll",
                                "System.Security.Cryptography.Primitives.dll",
                                "System.Security.Cryptography.X509Certificates.dll",
                                "System.Xml.ReaderWriter.dll",
                            })
                    }),

                NewtonsoftJson,

                // https://www.nuget.org/packages/WindowsAzure.Storage/7.2.1
                new Package("WindowsAzure.Storage",
                    new HashSet<string>()
                    {
                        "Microsoft.WindowsAzure.Storage.dll",
                    },
                    new HashSet<Package>()
                    {
                        NewtonsoftJson,

                        new Package("Flat.List.Of.Azure.Storage.Dependencies",
                            new HashSet<string>()
                            {
                                "Microsoft.Azure.KeyVault.Core.dll",
                                "Microsoft.Data.Edm.dll",
                                "Microsoft.Data.OData.dll",
                                "Microsoft.Data.Services.Client.dll",
                                "System.Spatial.dll",
                            })
                    }),
            });

        // https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions/2.1.0-beta1
        private static readonly Package WebJobsExtensions = new Package("Microsoft.Azure.WebJobs.Extensions",
            new HashSet<string>()
            {
                "Microsoft.Azure.WebJobs.Extensions.dll",
            },
            new HashSet<Package>()
            {
                WebJobs,

                // https://www.nuget.org/packages/Microsoft.Tpl.Dataflow/4.5.24
                new Package("Microsoft.Tpl.Dataflow",
                    new HashSet<string>()
                    {
                        "System.Threading.Tasks.Dataflow.dll",
                    }),

                // https://www.nuget.org/packages/ncrontab/3.3.0
                new Package("ncrontab",
                    new HashSet<string>()
                    {
                        "NCrontab.dll",
                    },
                    new HashSet<Package>()
                    {
                        new Package("Flat.List.Of.NCrontab.Dependencies",
                            new HashSet<string>()
                            {
                                "System.Collections.dll",
                                "System.Diagnostics.Debug.dll",
                                "System.Globalization.dll",
                                "System.IO.dll",
                                "System.Net.Primitives.dll",
                                "System.Resources.ResourceManager.dll",
                            })
                    })
            });

        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-csharp#referencing-external-assemblies
        private static readonly Package HostingEnvironment = new Package("HostingEnvironment",
            new HashSet<string>()
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll",
                "System.Xml.dll",
                "System.Net.Http.dll",
                "Microsoft.Azure.WebJobs",
                "Microsoft.Azure.WebJobs.Host",
                "Microsoft.Azure.WebJobs.Extensions",
                "System.Net.Http.Formatting.dll",
                "System.Web.Http.dll",
            });

        static readonly Package RootPackage = new Package("AzureFunction",
            new HashSet<string>(),
            new HashSet<Package>()
            {
                WebJobs,
                WebJobsExtensions,
                HostingEnvironment,
            });
    }
}
