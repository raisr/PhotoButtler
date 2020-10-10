using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Globalization;
using System.Linq;

namespace PhotoButtler
{
    internal class PhotoUtils
    {
        public static DateTime? GetDateTimeTaken(string file)
        {
            DateTime? dateTime = null;
            var subDirectory = ImageMetadataReader.ReadMetadata(file).OfType<ExifSubIfdDirectory>().FirstOrDefault();
            var dateTimeString = subDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

            if (dateTimeString != null)
            {
                dateTime = DateTime.ParseExact(dateTimeString, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture);
            }
            
            return dateTime;
        }
    }
}