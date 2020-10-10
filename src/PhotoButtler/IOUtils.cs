using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoButtler
{
    internal class IOUtils
    {
        public static IList<string> GetFilesIncludingSubfolders(string path, string searchPattern)
        {
            var paths = new List<string>();
            var directoriesQueue = new Queue<string>();
            directoriesQueue.Enqueue(path);

            while (directoriesQueue.Count > 0)
            {
                var currentPath = directoriesQueue.Dequeue();
                var directories = Directory.GetDirectories(currentPath);

                foreach (var directory in directories)
                {
                    directoriesQueue.Enqueue(directory);
                }

                paths.AddRange(Directory.GetFiles(currentPath, searchPattern).ToList());
            }

            return paths;
        }

        public static void CopyFileToDateFolder(string sourceFile, string destinationRootFolder, DateTime dateTimeTaken)
        {
            var dateTimeFolder = @$"{dateTimeTaken.Year:D4}\{dateTimeTaken.Month:D2}\{dateTimeTaken.Day:D2}";
            var destinationFile = Path.Combine(destinationRootFolder, dateTimeFolder, Path.GetFileName(sourceFile));
            var destinationPath = Path.GetDirectoryName(destinationFile);

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            File.Copy(sourceFile, destinationFile);
        }
    }
}