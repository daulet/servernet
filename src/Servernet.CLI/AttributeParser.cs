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
            var functionBuilder = new FunctionBuilder(functionType, functionMethod);

            object[] methodAttributes;
            try
            {
                methodAttributes = functionMethod.GetCustomAttributes(inherit: false);
            }
            catch (TypeLoadException e)
            {
                throw new ArgumentException(
                    $"Failed to load one of dependency assemblies for method '{functionMethod.Name}'",
                    functionMethod.Name, e);
            }

            foreach (var methodAttribute in methodAttributes)
            {
                functionBuilder.AddBinding(functionMethod, methodAttribute);
            }

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

            foreach (var parameter in parameters)
            {
                object[] parameterAttributes;
                try
                {
                    parameterAttributes = parameter.GetCustomAttributes(inherit: false);
                }
                catch (FileNotFoundException e)
                {
                    throw new ArgumentException(
                        $"Failed to load one of dependency assemblies for argument '{parameter.Name}'",
                        functionMethod.Name, e);
                }

                foreach (var parameteAttribute in parameterAttributes)
                {
                    // @TODO can't allow two bindings, but can have multiple custom attributes
                    functionBuilder.AddBinding(parameter, parameteAttribute);
                }
            }

            return functionBuilder.ToFunction();
        }
    }
}
