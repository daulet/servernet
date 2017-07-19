using System.IO;

namespace Servernet.Generator.Core
{
    internal class FileSystem : IFileSystem
    {
        public void CopyFile(string source, string destination, bool overwrite)
        {
            File.Copy(source, destination, overwrite);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public FileInfo[] GetFiles(string directoryPath)
        {
            return new DirectoryInfo(directoryPath).GetFiles();
        }

        public void WriteToFile(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }
    }
}