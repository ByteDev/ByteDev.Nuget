using System;
using NuGet.Versioning;

namespace ByteDev.Nuget
{
    /// <summary>
    /// Represents a nuget package file name.
    /// </summary>
    public static class NugetPackageFileName
    {
        /// <summary>
        /// Returns a package file name.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <returns>New package file name.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="id" /> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="version" /> is null.</exception>
        public static string Create(string id, NuGetVersion version)
        {
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            return Create(id, version.ToString());
        }

        /// <summary>
        /// Returns a package file name.
        /// </summary>
        /// <param name="id">Package ID.</param>
        /// <param name="version">Package version.</param>
        /// <returns>New package file name.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="id" /> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="version" /> is null or empty.</exception>
        public static string Create(string id, string version)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Package ID cannot be null or empty");

            if (string.IsNullOrEmpty(version))
                throw new ArgumentException("Package version cannot be null or empty");

            return $"{id}.{version}.nupkg";
        }
    }
}