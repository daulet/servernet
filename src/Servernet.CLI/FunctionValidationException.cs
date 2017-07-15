using System;

namespace Servernet.Generator
{
    internal class FunctionValidationException : Exception
    {
        public FunctionValidationException(string message)
            : base(message)
        { }
    }
}
