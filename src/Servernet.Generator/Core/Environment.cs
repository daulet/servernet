namespace Servernet.Generator.Core
{
    public class Environment : IEnvironment
    {
        public string CurrentDirectory => System.Environment.CurrentDirectory;
    }
}