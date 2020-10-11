using System;
using System.Collections.Generic;

namespace PhotoButtler
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceFolder = @"D:\dev-temp\original-images";
            var destinationFolder = @"D:\dev-temp\moved-images";

            var allImageFiles = IOUtils.GetFilesIncludingSubfolders(sourceFolder, "*.jpg");
            var filesWithTakenDateTime = new List<string>();
            var filesWithoutTakenDateTime = new List<string>();

            foreach (var file in allImageFiles)
            {
                var dateTimeTaken = PhotoUtils.GetDateTimeTaken(file);
                if (dateTimeTaken != null)
                {
                    IOUtils.MoveFileToDateFolder(file, destinationFolder, dateTimeTaken.Value);
                    filesWithTakenDateTime.Add(file);
                }
                else
                {
                    filesWithoutTakenDateTime.Add(file);
                }

                Console.WriteLine($"Files OK: {filesWithTakenDateTime.Count}\tFiles Error: {filesWithoutTakenDateTime.Count}");
            }
        }
    }
}
