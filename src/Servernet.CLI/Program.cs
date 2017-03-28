using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using Microsoft.Azure.WebJobs;

namespace Servernet.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine($"Assembly: {options.Assembly}");
                Console.WriteLine($"Type: {options.Type}");
                Console.WriteLine($"Function: {options.Function}");
                Console.WriteLine($"{Environment.CurrentDirectory}");

                var assemblyPath = Path.Combine(Environment.CurrentDirectory, options.Assembly);
                
                LoadFunction(assemblyPath, options.Type, options.Function);
            }
            else
            {
                Console.WriteLine("Invalid parameters");
            }

            Console.ReadKey();
        }

        private static void LoadFunction(string assemblyPath, string typeName, string functionName)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Failed to load assembly at location: {assemblyPath}");
                return;
            }

            Type type;
            try
            {
                type = assembly.GetType(typeName, throwOnError: true, ignoreCase: true);
            }
            catch (TypeLoadException)
            {
                Console.WriteLine($"Failed to load type with name: {typeName}");
                return;
            }

            MethodInfo methodInfo;
            try
            {
                methodInfo = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public);
            }
            catch (AmbiguousMatchException)
            {
                Console.WriteLine($"Failed to find unique method with name: {functionName}");
                return;
            }

            ParameterInfo[] parameters;
            try
            {
                parameters = methodInfo.GetParameters();
            }
            catch (FileLoadException e)
            {
                Console.WriteLine($"Failed to load one of dependency assemblies for method '{methodInfo.Name}': {e}");
                return;
            }
            
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
                    Console.WriteLine($"Found {attribute} on parameter {parameter.Name}");

                    LoadAttribute(attribute);
                }
            }
        }

        private static void LoadAttribute(object attribute)
        {
            var typeSwitch = new TypeSwitch()
                .Case((QueueTriggerAttribute x) => { Console.WriteLine(x.QueueName);});

            typeSwitch.Switch(attribute);
        }
    }
}
