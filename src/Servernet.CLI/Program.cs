using System;
using CommandLine;

namespace Servernet.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine($"Assembly: {options.Assembly}");
                Console.WriteLine($"Type: {options.Type}");
                Console.WriteLine($"Function: {options.Function}");
            }
            else
            {
                Console.WriteLine("Invalid parameters");
            }

            Console.ReadKey();
        }
    }
}
