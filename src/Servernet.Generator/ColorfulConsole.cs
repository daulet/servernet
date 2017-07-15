using System;

namespace Servernet.Generator
{
    internal class ColorfulConsole : ILogger
    {
        public void Error(string message)
        {
            WriteWithForeground(message, ConsoleColor.Red);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Verbose(string message)
        {
#if DEBUG
            WriteWithForeground(message, ConsoleColor.DarkGray);
#endif
        }

        public void Warning(string message)
        {
            WriteWithForeground(message, ConsoleColor.Yellow);
        }

        private void WriteWithForeground(string message, ConsoleColor foregroundColor)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;

            Console.WriteLine(message);

            Console.ForegroundColor = currentColor;
        }
    }
}