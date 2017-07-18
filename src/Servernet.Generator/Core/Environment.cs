namespace Servernet.Generator.Core
{
    internal class Environment : IEnvironment
    {
        public string CurrentDirectory => System.Environment.CurrentDirectory;
    }
}