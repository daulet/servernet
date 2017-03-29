using System;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class HttpTriggerAttribute : Attribute
    {
        // @TODO route as constructor parameter
        // @TODO HttpRoute attribute to dynamically parse and assign HttpTrigger route parameters
        // @TODO decide how to define accepted HTTP method
    }
}
