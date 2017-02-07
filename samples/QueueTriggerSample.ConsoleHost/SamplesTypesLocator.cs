using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;

namespace QueueTriggerSample.ConsoleHost
{
    internal class SamplesTypeLocator : ITypeLocator
    {
        private Type[] _types;

        public SamplesTypeLocator(params Type[] types)
        {
            _types = types;
        }

        public IReadOnlyList<Type> GetTypes()
        {
            return _types;
        }
    }
}
