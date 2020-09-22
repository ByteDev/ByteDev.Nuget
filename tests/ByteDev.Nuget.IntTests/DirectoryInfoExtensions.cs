using System.IO;

namespace ByteDev.Nuget.IntTests
{
    internal static class DirectoryInfoExtensions
    {
        public static void DeleteIfExists(this DirectoryInfo source, bool recursive)
        {
            try
            {
                source.Delete(true);
            }
            catch (DirectoryNotFoundException)
            {
                // Swallow exception
            }
        }
    }
}