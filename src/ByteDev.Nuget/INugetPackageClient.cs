using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ByteDev.Nuget.Contract;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace ByteDev.Nuget
{
    public interface INugetPackageClient
    {
        /// <summary>
        /// Download a package and return as a stream.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<Stream> DownloadToStreamAsync(string id, string version, CancellationToken cancellationToken = default);

        /// <summary>
        /// Download a package and save to file.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="filePath">File path to save the file to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DownloadToFileAsync(string id, string version, string filePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve metadata for all the versions of a given package.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IEnumerable<IPackageSearchMetadata>> GetAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a package's metadata.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IPackageSearchMetadata> GetAsync(string id, string version, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a package's metadata.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IPackageSearchMetadata> GetAsync(string id, NuGetVersion version, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a package's metadata.
        /// </summary>
        /// <param name="packageIdentity">Package identity.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IPackageSearchMetadata> GetAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve all version information for a package.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IEnumerable<NuGetVersion>> GetVersionsAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the latest version metadata of a package.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IPackageSearchMetadata> GetLatestAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Perform a package search.
        /// </summary>
        /// <param name="request">Search request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IEnumerable<IPackageSearchMetadata>> SearchAsync(SearchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determine if a package exists in nuget.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determine if a package version exists in nuget.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> ExistsAsync(string id, string version, CancellationToken cancellationToken = default);

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
        Task<IList<IPackageSearchMetadata>> GetDependenciesAsync(GetDependenciesRequest request, CancellationToken cancellationToken = default);
    }
}