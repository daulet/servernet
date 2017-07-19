using System.IO;

namespace Servernet.Generator
{
    public interface IFileSystem
    {
        void CopyFile(string source, string destination, bool overwrite);

        void CreateDirectory(string path);

        FileInfo[] GetFiles(string directoryPath);

        void WriteToFile(string filePath, string text);
    }
}