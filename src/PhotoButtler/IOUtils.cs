using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhotoButtler
{
    internal class IOUtils
    {
        private const string FileVersionPattern = @"(?<fileName>.*?)(?<versionSuffix>\s-\s\((?<version>\d)\))";
        private const string VersionSuffixFormatString = " - ({0})";

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

            var retry = true;

            while (retry)
            {
                try
                {
                    File.Copy(sourceFile, destinationFile);
                    retry = false;
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("exists"))
                    {
                        destinationFile = GetNextFilenameVersion(destinationFile);
                    }
                }
            }
        }

        private static string GetNextFilenameVersion(string file)
        {
            var version = 1;

            var fileName = Path.GetFileName(file);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            var match = Regex.Match(fileNameWithoutExtension, FileVersionPattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                version = Int32.Parse(match.Groups["version"].Value) + 1;
                fileNameWithoutExtension = match.Groups["fileName"].Value;
            }

            fileNameWithoutExtension = $"{fileNameWithoutExtension}{string.Format(VersionSuffixFormatString, version)}";

            var newDestinationFile = $@"{Path.GetDirectoryName(file)}\{fileNameWithoutExtension}{Path.GetExtension(fileName)}";

            return newDestinationFile;
        }
    }
}