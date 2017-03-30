using CommandLine;
using CommandLine.Text;

namespace Servernet.CLI
{
    internal class Options
    {
        [Option('a', "Assembly", Required = true,
            HelpText = "Path to the assembly that contains the entry function")]
        public string Assembly { get; set; }

        [Option('f', "Function", Required = false,
            HelpText = "Fully-qualified method name of the entry function, e.g. 'System.String.Compare' for Compare function")]
        public string Function { get; set; }

        [Option('o', "Output", Required = false,
            HelpText = "Output directory")]
        public string OutputDirectory { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
