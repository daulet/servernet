using System;
using System.IO;
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
                
                LoadFunction(assemblyPath, options.Type, options.Function);
            }
            else
            {
                Console.WriteLine("Invalid parameters");
            }

            Console.ReadKey();
        }

        private static void LoadFunction(string assemblyPath, string typeName, string functionName, string outputDirectory = null)
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

            if (outputDirectory == null)
            {
                outputDirectory = functionType.Name;
            }

            Directory.CreateDirectory(outputDirectory);

            using (var bindingFile = new StreamWriter($"{outputDirectory}/function.json"))
            {
                bindingFile.Write(functionBuilder.ToString());
            }

            var assemblyFileName = Path.GetFileName(functionAssembly.Location);
            File.Copy(functionAssembly.Location, Path.Combine(outputDirectory, assemblyFileName), true);
        }
    }
}
