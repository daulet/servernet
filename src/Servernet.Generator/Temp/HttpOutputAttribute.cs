using System;

namespace Servernet.Generator.Temp
{
    /// <summary>
    /// Temporary, until WebJobs Extensions provides this functionality:
    /// https://github.com/Azure/azure-webjobs-sdk-extensions/issues/251
    /// </summary>
    [AttributeUsage(AttributeTargets.ReturnValue)]
    public sealed class HttpOutputAttribute : Attribute
    {
    }
}
