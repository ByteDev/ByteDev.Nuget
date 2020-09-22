using System;
using NuGet.Frameworks;

namespace ByteDev.Nuget.Factories
{
    /// <summary>
    /// Represents a factory of <see cref="T:NuGet.Frameworks.NuGetFramework" />.
    /// </summary>
    public static class NuGetFrameworkFactory
    {
        public static NuGetFramework Create(string frameworkName)
        {
            return frameworkName == null
                ? NuGetFramework.AnyFramework
                : NuGetFramework.ParseFrameworkName(frameworkName, new DefaultFrameworkNameProvider());
        }

        public static NuGetFramework CreateStandard(string version)
        {
            return new NuGetFramework(".NETStandard", new Version(version));
        }

        public static NuGetFramework CreateFramework(string version)
        {
            return new NuGetFramework(".NETFramework", new Version(version));
        }
    }
}