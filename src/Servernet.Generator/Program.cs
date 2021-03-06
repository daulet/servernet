﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Servernet.Generator
{
    public partial class Program
    {
        private readonly AttributeParser _attributeParser;
        private readonly IEnvironment _environment;
        private readonly FunctionLocator _functionLocator;
        private readonly FunctionValidator _functionValidator;
        private readonly ILogger _log;
        private readonly MethodLocator _methodLocator;
        private readonly Options _options;
        private readonly ReleaseBuilder _releaseBuilder;

        public Program(
            IAssemblyLoader assemblyLoader,
            IEnvironment environment,
            IFileSystem fileSystem,
            ILogger log,
            Options options)
            : this(
                  new AttributeParser(),
                  environment,
                  new FunctionLocator(assemblyLoader, log),
                  new FunctionValidator(log),
                  log,
                  new MethodLocator(assemblyLoader),
                  options,
                  new ReleaseBuilder(fileSystem))
        { }

        private Program(
            AttributeParser attributeParser,
            IEnvironment environment,
            FunctionLocator functionLocator,
            FunctionValidator functionValidator,
            ILogger log,
            MethodLocator methodLocator,
            Options options,
            ReleaseBuilder releaseBuilder)
        {
            _attributeParser = attributeParser;
            _environment = environment;
            _functionLocator = functionLocator;
            _functionValidator = functionValidator;
            _log = log;
            _methodLocator = methodLocator;
            _options = options;
            _releaseBuilder = releaseBuilder;
        }

        public void Run()
        {
            var assemblyPath = Path.Combine(_environment.CurrentDirectory, _options.AssemblyPath);

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
                var sourceDirectory = Path.GetDirectoryName(assemblyPath);
                var targetDirectory = Path.Combine(_options.OutputDirectory?? string.Empty, functionDefinition.Name);
                _releaseBuilder.Release(sourceDirectory, targetDirectory, functionDefinition);
            }
        }
    }
}
