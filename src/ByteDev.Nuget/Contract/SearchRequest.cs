using System;

namespace ByteDev.Nuget.Contract
{
    /// <summary>
    /// Represents a package search request.
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Search term.
        /// </summary>
        public string SearchTerm { get; }

        /// <summary>
        /// Determines whether to include pre-release packages.
        /// </summary>
        public bool IncludePreRelease { get; set; } = false;

        /// <summary>
        /// Number of results to skip, for pagination. Default 0.
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Number of results to return, for pagination. Default 20.
        /// </summary>
        public int Take { get; set; } = 20;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.Contract.SearchPackagesQuery" /> class.
        /// </summary>
        /// <param name="searchTerm">Search term.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="searchTerm" /> is null or empty.</exception>
        public SearchRequest(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                throw new ArgumentException("Search term cannot be null or empty.");

            SearchTerm = searchTerm;
        }
    }
}