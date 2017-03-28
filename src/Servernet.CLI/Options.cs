using CommandLine;
using CommandLine.Text;

namespace Servernet.CLI
{
    internal class Options
    {
        [Option('a', "Assembly", Required = true,
            HelpText = "Name of the assembly that contains entry function")]
        public string Assembly { get; set; }

        [Option('t', "Type", Required = true,
            HelpText = "Name of the type that defines the entry function")]
        public string Type { get; set; }

        [Option('f', "Function", Required = true,
            HelpText = "Name of the entry function")]
        public string Function { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
