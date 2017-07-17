using System.Reflection;

namespace Servernet.Generator
{
    public interface IAssemblyLoader
    {
        Assembly LoadFrom(string assemblyPath);
    }
}