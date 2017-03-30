using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Servernet.CLI
{
    internal partial class Program
    {
        private readonly AttributeParser _attributeParser;
        private readonly ILogger _log;
        private readonly MethodLocator _methodLocator;
        private readonly Options _options;
        private readonly ReleaseBuilder _releaseBuilder;

        private Program(
            AttributeParser attributeParser,
            ILogger log,
            MethodLocator methodLocator,
            Options options,
            ReleaseBuilder releaseBuilder)
        {
            _attributeParser = attributeParser;
            _log = log;
            _methodLocator = methodLocator;
            _options = options;
            _releaseBuilder = releaseBuilder;
        }

        private void Run()
        {
            var assemblyPath = Path.Combine(Environment.CurrentDirectory, _options.Assembly);
            var foundFunctions = new HashSet<Tuple<Type, MethodInfo>>();

            if (string.IsNullOrEmpty(_options.Function))
            {
                // find all [AzureFunction] attributes
            }
            else
            {
                var fullFunctionName = _options.Function;
                // assuming function name can't contain a period
                var indexOfLastPeriod = fullFunctionName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
                if (indexOfLastPeriod <= 0)
                {
                    Console.WriteLine($"Invalid fully qualified method name: {fullFunctionName}");
                    return;
                }
                var typeName = fullFunctionName.Substring(0, indexOfLastPeriod);
                var methodName = fullFunctionName.Substring(indexOfLastPeriod + 1);

                // Locate a method

                var locatedMethod = _methodLocator.Locate(assemblyPath, typeName, methodName);
                foundFunctions.Add(locatedMethod);
            }

            foreach (var function in foundFunctions)
            {
                var functionType = function.Item1;
                var functionMethod = function.Item2;

                // Generate bindings

                var functionBuilder = _attributeParser.ParseEntryPoint(functionType, functionMethod);
                functionBuilder.Validate(_log);

                // Generate output

                var outputDirectory = string.IsNullOrEmpty(_options.OutputDirectory)
                    ? functionType.Name
                    : _options.OutputDirectory;
                var sourceDirectory = new FileInfo(functionType.Assembly.Location).Directory;
                var targetDirectory = new DirectoryInfo(outputDirectory);
                _releaseBuilder.Release(sourceDirectory, targetDirectory, functionBuilder);
            }
        }
    }
}
