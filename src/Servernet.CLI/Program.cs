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

            // Locate a method

            var locator = new MethodLocator();
            var locatedMethod = locator.Locate(assemblyPath, typeName, methodName);
            var functionType = locatedMethod.Item1;
            var functionMethod = locatedMethod.Item2;

            // Generate bindings

            var parser = new AttributeParser();
            var functionBuilder = parser.ParseEntryPoint(functionType, functionMethod);
            functionBuilder.Validate(_log);

            // Generate output

            var outputDirectory = string.IsNullOrEmpty(_options.OutputDirectory)
                ? functionType.Name
                : _options.OutputDirectory;
            var sourceDirectory = new FileInfo(functionType.Assembly.Location).Directory;
            var targetDirectory = new DirectoryInfo(outputDirectory);
            var releaseBuilder = new ReleaseBuilder();
            releaseBuilder.Release(sourceDirectory, targetDirectory, functionBuilder);
        }
    }
}
