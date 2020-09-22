using System;
using System.Collections.Generic;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace ByteDev.Nuget.Factories
{
    internal static class SourceRepositoryFactory
    {
        public static SourceRepository Create()
        {
            var packageSource = new PackageSource("https://api.nuget.org/v3/index.json", "NugetV3");

            var providers = new List<Lazy<INuGetResourceProvider>>();

            providers.AddRange(Repository.Provider.GetCoreV3());

            return new SourceRepository(packageSource, providers);
        }
    }
}