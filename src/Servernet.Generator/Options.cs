﻿using CommandLine;
using CommandLine.Text;

namespace Servernet.Generator
{
    public class Options
    {
        // @TODO if AssemblyPath is relative, use CurrentDirectory, otherwise accept absolute path

        [Option('a', "AssemblyPath", Required = true,
            HelpText = "Path to the assembly that contains the entry function")]
        public string AssemblyPath { get; set; }

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
