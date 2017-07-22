using System;
using System.Diagnostics;
using System.IO;

namespace Servernet.Generator.UnitTests
{
    public static class PathExtensions
    {
        public static bool IsEqual(string path1, string path2)
        {
            try
            {
                return string.Equals(NormalizedPath(path1), NormalizedPath(path2));
            }
            catch (UriFormatException)
            {
                return string.Equals(path1, path2);
            }
        }

        private static string NormalizedPath(string path)
        {
            return Path
                .GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
    }
}