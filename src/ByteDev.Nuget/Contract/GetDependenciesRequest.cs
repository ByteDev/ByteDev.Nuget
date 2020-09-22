using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using NuGet.Protocol.Core.Types;

namespace ByteDev.Nuget.Contract
{
    /// <summary>
    /// Represents a package dependencies request.
    /// </summary>
    public class GetDependenciesRequest
    {
        private IList<string> _ignoreAuthors;

        internal IPackageSearchMetadata RootPackage { get; set; }

        /// <summary>
        /// True add the root package to the returned results; other don't.
        /// </summary>
        public bool AddRootPackageToResults { get; set; } = false;

        /// <summary>
        /// Root package ID.
        /// </summary>
        public string PackageId { get; }

        /// <summary>
        /// Root package version.
        /// </summary>
        public string PackageVersion { get; }

        /// <summary>
        /// Root package.
        /// </summary>
        public IPackageSearchMetadata Package { get; }

        /// <summary>
        /// Target framework filter applied to all returned dependencies.
        /// </summary>
        public NuGetFramework DependencyTarget { get; }

        /// <summary>
        /// Collection of authors to ignore. Values are case sensitive.
        /// </summary>
        public IList<string> IgnoreAuthors
        {
            get => _ignoreAuthors ?? (_ignoreAuthors = new List<string>());
            set => _ignoreAuthors = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.Contract.GetPackageDependenciesQuery" /> class.
        /// </summary>
        /// <param name="packageId">Root package ID.</param>
        /// <param name="packageVersion">Root package version.</param>
        /// <param name="dependencyTarget">Target framework filter applied to all returned dependencies.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="packageId" /> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="packageVersion" /> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dependencyTarget" /> is null.</exception>
        public GetDependenciesRequest(string packageId, string packageVersion, NuGetFramework dependencyTarget)
        {
            if (string.IsNullOrEmpty(packageId))
                throw new ArgumentException("Package ID cannot be null or empty.");

            if (string.IsNullOrEmpty(packageVersion))
                throw new ArgumentException("Package version cannot be null or empty.");

            PackageId = packageId;
            PackageVersion = packageVersion;
            DependencyTarget = dependencyTarget ?? throw new ArgumentNullException(nameof(dependencyTarget));;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.Contract.GetPackageDependenciesQuery" /> class.
        /// </summary>
        /// <param name="package">Root package.</param>
        /// <param name="dependencyTarget">Target framework filter applied to all returned dependencies.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="package" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dependencyTarget" /> is null.</exception>
        public GetDependenciesRequest(IPackageSearchMetadata package, NuGetFramework dependencyTarget)
        {
            Package = package ?? throw new ArgumentNullException(nameof(package));
            DependencyTarget = dependencyTarget ?? throw new ArgumentNullException(nameof(dependencyTarget));
        }
    }
}