using System.Reflection;

namespace Servernet.Generator.Core
{
    internal class AssemblyLoader : IAssemblyLoader
    {
        public Assembly LoadFrom(string assemblyPath)
        {
            return Assembly.LoadFrom(assemblyPath);
        }
    }
}