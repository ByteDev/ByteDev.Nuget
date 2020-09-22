using System;
using System.IO;
using System.Threading.Tasks;

namespace ByteDev.Nuget
{
    public static class StreamExtensions
    {
        public static bool IsEmpty(this Stream source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Length == 0;
        }

        public static async Task WriteToFileAsync(this Stream source, string filePath)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (Stream file = File.Create(filePath))
            {
                source.Seek(0, SeekOrigin.Begin);
                await source.CopyToAsync(file);
            }
        }
    }
}