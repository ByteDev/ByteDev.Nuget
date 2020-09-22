using System.Linq;
using System.Reflection;

namespace ByteDev.Nuget
{
    internal static class AssemblyExtensions
    {
        public static string GetFrameworkName(this Assembly source)
        {
            // e.g. ".NETCoreApp,Version=v3.1"
            return source.GetCustomAttributes(true)
                .OfType<System.Runtime.Versioning.TargetFrameworkAttribute>()
                .Select(x => x.FrameworkName)
                .FirstOrDefault();
        }

        // Assembly x = Assembly.GetExecutingAssembly();
        // var framework = NuGetFrameworkFactory.Create(x.GetFrameworkName());
    }
}