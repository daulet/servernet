using System.IO;

namespace Servernet.Generator
{
    public interface IFileSystem
    {
        void CopyFile(string source, string destination, bool overwrite);

        void CreateDirectory(string path);

        TextWriter CreateFileWriter(string filePath);

        FileInfo[] GetFiles(string directoryPath);
    }
}