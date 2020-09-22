using NuGet.Protocol.Core.Types;

namespace ByteDev.Nuget.Factories
{
    internal static class SourceCacheContextFactory
    {
        public static SourceCacheContext Create()
        {
            return new SourceCacheContext
            {
                NoCache = true,
                DirectDownload = true
            };
        }
    }
}