using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Servernet.Generator
{
    public partial class Program
    {
        private readonly AttributeParser _attributeParser;
        private readonly FunctionLocator _functionLocator;
        private readonly FunctionValidator _functionValidator;
        private readonly ILogger _log;
        private readonly MethodLocator _methodLocator;
        private readonly Options _options;
        private readonly ReleaseBuilder _releaseBuilder;

        public Program(
            ILogger log,
            Options options)
            : this(
                  new AttributeParser(),
                  new FunctionLocator(log),
                  new FunctionValidator(log),
                  log,
                  new MethodLocator(),
                  options,
                  new ReleaseBuilder())
        { }

        private Program(
            AttributeParser attributeParser,
            FunctionLocator functionLocator,
            FunctionValidator functionValidator,
            ILogger log,
            MethodLocator methodLocator,
            Options options,
            ReleaseBuilder releaseBuilder)
        {
            _attributeParser = attributeParser;
            _functionLocator = functionLocator;
            _functionValidator = functionValidator;
            _log = log;
            _methodLocator = methodLocator;
            _options = options;
            _releaseBuilder = releaseBuilder;
        }

        private void Run()
        {
            var assemblyPath = Path.Combine(Environment.CurrentDirectory, _options.Assembly);

            HashSet<Tuple<Type, MethodInfo>> allFunctions;
            if (string.IsNullOrEmpty(_options.Function))
            {
                var foundFunctions = _functionLocator.Locate(assemblyPath);
                allFunctions = new HashSet<Tuple<Type, MethodInfo>>(foundFunctions);
            }
            else
            {
                var fullFunctionName = _options.Function;
                // assuming function name can't contain a period
                var indexOfLastPeriod = fullFunctionName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
                if (indexOfLastPeriod <= 0)
                {
                    throw new ArgumentException($"Invalid fully qualified method name: {fullFunctionName}");
                }
                var typeName = fullFunctionName.Substring(0, indexOfLastPeriod);
                var methodName = fullFunctionName.Substring(indexOfLastPeriod + 1);

                // Locate a method

                var locatedMethod = _methodLocator.Locate(assemblyPath, typeName, methodName);
                allFunctions = new HashSet<Tuple<Type, MethodInfo>>()
                {
                    locatedMethod,
                };
            }

            foreach (var function in allFunctions)
            {
                var functionType = function.Item1;
                var functionMethod = function.Item2;

                _log.Info($"Parsing {functionType.FullName}.{functionMethod.Name}");

                // Generate bindings
                var functionDefinition = _attributeParser.ParseEntryPoint(functionType, functionMethod);

                // Validate function
                try
                {
                    _functionValidator.Validate(functionDefinition);
                }
                catch (FunctionValidationException e)
                {
                    _log.Error($"{functionDefinition.EntryPoint} failed validation: {e.Message}");
                    continue;
                }

                // Generate output
                var sourceDirectory = new FileInfo(functionType.Assembly.Location).Directory;
                var targetDirectory = new DirectoryInfo(Path.Combine(_options.OutputDirectory?? string.Empty, functionDefinition.Name));
                _releaseBuilder.Release(sourceDirectory, targetDirectory, functionDefinition);
            }
        }
    }
}
