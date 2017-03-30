using System;
using System.Collections.Generic;
using System.Linq;
using Servernet.CLI.Definition;

namespace Servernet.CLI
{
    internal class FunctionValidator
    {
        private readonly ILogger _log;

        public FunctionValidator(ILogger log)
        {
            _log = log;
        }

        public void Validate(Function function)
        {
            try
            {
                function.Bindings
                    .Single(x => ((int)x.Type & BindingCategory.Trigger) > 0);
            }
            catch (InvalidOperationException)
            {
                throw new FunctionValidationException($"{function.EntryPoint} must have exactly one trigger binding");
            }

            if (function.Bindings.OfType<HttpTriggerBinding>().Any())
            {
                if (!function.Bindings.OfType<HttpOutputBinding>().Any())
                {
                    _log.Warning(
                        "If an HTTP output binding is not provided, " +
                        "an HTTP trigger will return HTTP 200 OK with an empty body.");
                }
            }

            if (function.Bindings.OfType<HttpOutputBinding>().Any())
            {
                if (!function.Bindings.OfType<HttpTriggerBinding>().Any())
                {
                    _log.Error("HttpOutput attribute requires an HTTP trigger " +
                        "and allows you to customize the response associated with the trigger's request");
                }
            }
        }
    }
}
