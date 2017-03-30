using System;
using CommandLine;

namespace Servernet.CLI
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            ILogger log = new ColorfulConsole();
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                var program = new Program(
                    new AttributeParser(),
                    new FunctionLocator(log),
                    new FunctionValidator(log), 
                    log,
                    new MethodLocator(),
                    options,
                    new ReleaseBuilder());
                try
                {
                    program.Run();
                }
                catch (ArgumentException e)
                {
#if DEBUG
                    log.Error(e.ToString());
#else
                    log.Error(e.Message);
#endif
                }
            }
            else
            {
                log.Error("Invalid parameters");
            }
        }
    }
}
