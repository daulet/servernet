using System;

namespace Servernet.CLI
{
    internal class FunctionValidationException : Exception
    {
        public FunctionValidationException(string message)
            : base(message)
        { }
    }
}
