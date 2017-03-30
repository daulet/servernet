using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Servernet.CLI.Definition;

namespace Servernet.CLI
{
    internal class AttributeParser
    {
        public Function ParseEntryPoint(Type functionType, MethodInfo functionMethod)
        {
            ParameterInfo[] parameters;
            try
            {
                parameters = functionMethod.GetParameters();
            }
            catch (FileLoadException e)
            {
                throw new ArgumentException(
                    $"Failed to load one of dependency assemblies for method '{functionMethod.Name}'", 
                    functionMethod.Name, e);
            }

            var functionBuilder = new FunctionBuilder(functionType, functionMethod);

            foreach (var parameter in parameters)
            {
                object[] attributes;
                try
                {
                    attributes = parameter.GetCustomAttributes(inherit: false);
                }
                catch (FileNotFoundException e)
                {
                    throw new ArgumentException(
                        $"Failed to load one of dependency assemblies for argument '{parameter.Name}'",
                        functionMethod.Name, e);
                }

                foreach (var attribute in attributes)
                {
                    // @TODO can't allow two bindings, but can have multiple custom attributes
                    functionBuilder.AddBinding(parameter, attribute);
                }
            }

            return functionBuilder.ToFunction();
        }
    }
}
