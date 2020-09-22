using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ByteDev.Nuget.Contract;
using ByteDev.Nuget.Factories;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace ByteDev.Nuget
{
    /// <summary>
    /// Represents a client for package operations against the nuget API.
    /// </summary>
    public class NugetPackageClient : INugetPackageClient
    {
        private readonly ILogger _logger;
        private readonly SourceRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.NugetPackageClient" /> class.
        /// </summary>
        public NugetPackageClient() : this(NullLogger.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.NugetPackageClient" /> class.
        /// </summary>
        /// <param name="logger">Logger for nuget API operations.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="logger" /> is null.</exception>
        public NugetPackageClient(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _repository = SourceRepositoryFactory.Create();
        }

        /// <summary>
        /// Download a package and return as a stream.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:ByteDev.Nuget.NugetPackageNotFoundException">Package could not be found.</exception>
        public async Task<Stream> DownloadToStreamAsync(string id, string version, CancellationToken cancellationToken = default)
        {
            var resource = await _repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);

            using (var context = SourceCacheContextFactory.Create())
            {
                Stream destinationStream = new MemoryStream();
                await resource.CopyNupkgToStreamAsync(id, NuGetVersion.Parse(version), destinationStream, context, _logger, cancellationToken);

                if (destinationStream.IsEmpty())
                {
                    destinationStream.Dispose();
                    throw new NugetPackageNotFoundException(id, version);
                }

                return destinationStream;
            }
        }

        /// <summary>
        /// Download a package and save to file.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="filePath">File path to save the file to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:ByteDev.Nuget.NugetPackageNotFoundException">Package could not be found.</exception>
        public async Task DownloadToFileAsync(string id, string version, string filePath, CancellationToken cancellationToken = default)
        {
            using (var stream = await DownloadToStreamAsync(id, version, cancellationToken))
            {
                await stream.WriteToFileAsync(filePath);
            }
        }

        /// <summary>
        /// Retrieve metadata for all the versions of a given package.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<IEnumerable<IPackageSearchMetadata>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var resource = await _repository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            using (var context = SourceCacheContextFactory.Create())
            {
                // TODO: add options
                return await resource.GetMetadataAsync(id, true, true, context, _logger, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieve a package's metadata.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<IPackageSearchMetadata> GetAsync(string id, string version, CancellationToken cancellationToken = default)
        {
            return GetAsync(id, NuGetVersion.Parse(version), cancellationToken);
        }

        /// <summary>
        /// Retrieve a package's metadata.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<IPackageSearchMetadata> GetAsync(string id, NuGetVersion version, CancellationToken cancellationToken = default)
        {
            var packageIdentity = new PackageIdentity(id, version);

            return GetAsync(packageIdentity, cancellationToken);
        }

        /// <summary>
        /// Retrieve a package's metadata.
        /// </summary>
        /// <param name="packageIdentity">Package identity.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<IPackageSearchMetadata> GetAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken = default)
        {
            var resource = await _repository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            using (var context = SourceCacheContextFactory.Create())
            {
                return await resource.GetMetadataAsync(packageIdentity, context, _logger, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieve all version information for a package.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<IEnumerable<NuGetVersion>> GetVersionsAsync(string id, CancellationToken cancellationToken = default)
        {
            var resource = await _repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);

            using (var context = SourceCacheContextFactory.Create())
            {
                return await resource.GetAllVersionsAsync(id, context, _logger, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieve the latest version metadata of a package.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<IPackageSearchMetadata> GetLatestAsync(string id, CancellationToken cancellationToken = default)
        {
            var versions = await GetVersionsAsync(id, cancellationToken);

            NuGetVersion latestVersion = versions.OrderBy(v => v.Version).LastOrDefault();

            if (latestVersion == null)
                return null;

            return await GetAsync(id, latestVersion, cancellationToken);
        }

        /// <summary>
        /// Perform a package search.
        /// </summary>
        /// <param name="request">Search request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="request" /> is null.</exception>
        public async Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(SearchRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var resource = await _repository.GetResourceAsync<PackageSearchResource>(cancellationToken);
            
            return await resource.SearchAsync(request.SearchTerm, 
                new SearchFilter(request.IncludePreRelease), 
                request.Skip, 
                request.Take,
                _logger, 
                cancellationToken);
        }

        /// <summary>
        /// Determine if a package exists in nuget.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await GetAsync(id, cancellationToken);

            return result.Any();
        }

        /// <summary>
        /// Determine if a package version exists in nuget.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<bool> ExistsAsync(string id, string version, CancellationToken cancellationToken = default)
        {
            var resource = await _repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);

            using (var context = SourceCacheContextFactory.Create())
            {
                return await resource.DoesPackageExistAsync(id, NuGetVersion.Parse(version), context, _logger, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieve dependency package metadata for a package.
        /// </summary>
        /// <remarks>
        /// If a package has a large number of dependencies this method can be long running.
        /// Use the IgnoreAuthors filter on the request object to filter out unwanted dependencies.
        /// </remarks>
        /// <param name="request">Get dependencies request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<IList<IPackageSearchMetadata>> GetDependenciesAsync(GetDependenciesRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await SetQueryRootPackage(request, cancellationToken);

            var list = new List<IPackageSearchMetadata>();

            if (request.RootPackage == null)
                return list;

            if (request.AddRootPackageToResults)
                list.Add(request.RootPackage);

            await AddDependencies(list, request, request.RootPackage, cancellationToken);

            return list;
        }

        private async Task SetQueryRootPackage(GetDependenciesRequest request, CancellationToken cancellationToken)
        {
            if (request.Package == null)
                request.RootPackage = await GetAsync(request.PackageId, request.PackageVersion, cancellationToken);
            else
                request.RootPackage = request.Package;
        }

        private async Task AddDependencies(List<IPackageSearchMetadata> list, 
            GetDependenciesRequest request,
            IPackageSearchMetadata package, 
            CancellationToken cancellationToken)
        {
            foreach (var dependencyGroup in package.DependencySets.Where(x => x.TargetFramework == request.DependencyTarget))
            {
                foreach (var dependencyPackage in dependencyGroup.Packages.OrderBy(p => p.Id))
                {
                    var p = await GetAsync(dependencyPackage.Id, dependencyPackage.VersionRange.MinVersion.ToString(), cancellationToken);

                    if (!p.Authors.ContainsAny(request.IgnoreAuthors))
                    {
                        list.Add(p);
                        await AddDependencies(list, request, p, cancellationToken);
                    }
                }
            }
        }
    }
}