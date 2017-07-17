using System;
using CommandLine;
using Servernet.Generator.Core;

namespace Servernet.Generator
{
    public partial class Program
    {
        private static void Main(string[] args)
        {
            ILogger log = new ColorfulConsole();
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                var program = new Program(
                    new Core.AssemblyLoader(),
                    new Core.Environment(),
                    log,
                    options);
                try
                {
                    program.Run();
                }
                catch (ArgumentException e)
                {
                    log.Error(e.Message);
                    log.Verbose($"Exception: {e.ToString()}");
                }
            }
            else
            {
                log.Error("Invalid parameters");
            }
        }
    }
}
