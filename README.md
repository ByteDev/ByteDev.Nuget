[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.Nuget?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-Nuget/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.Nuget.svg)](https://www.nuget.org/packages/ByteDev.Nuget)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/ByteDev/ByteDev.Nuget/blob/master/LICENSE)

# ByteDev.Nuget

.NET Standard library that provides some nuget package related functionality against the nuget V3 API.

The library is built on top of the [NuGet Client SDK](https://docs.microsoft.com/en-us/nuget/reference/nuget-client-sdk).

## Installation

ByteDev.Nuget has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.Nuget is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.Nuget`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.Nuget/).

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.Nuget/blob/master/docs/RELEASE-NOTES.md).

## Usage

`NugetPackageClient` is the main type for performing package operations:

Methods:

- DownloadToFileAsync
- DownloadToStreamAsync
- GetAsync
- GetDependenciesAsync
- GetLatestAsync
- GetVersionsAsync
- ExistsAsync
- SearchAsync

```csharp
// Initialize the client type
INugetPackageClient client = new NugetPackageClient();
```

```csharp
// Download a package to a file
string fileName = NugetPackageFileName.Create("Newtonsoft.Json", "12.0.3");
string filePath = Path.Combine("C:\Temp", fileName);

await client.DownloadToFileAsync("Newtonsoft.Json", "12.0.3", filePath);


// Download a package to a stream
using(Stream stream = await client.DownloadToStreamAsync("Newtonsoft.Json", "12.0.3"))
{
    // use stream...
}
```

```csharp
// Get all package versions
IEnumerable<IPackageSearchMetadata> packages = await client.GetAsync("Newtonsoft.Json");


// Get single package version
IPackageSearchMetadata package = await client.GetAsync("Newtonsoft.Json", "12.0.3");


// Get a package's dependencies
var nuGetFramework = NuGetFrameworkFactory.CreateFramework("4.5");

var query = new GetPackageDependenciesRequest("Moq", "4.14.5", nuGetFramework)
{
    AddRootPackageToResults = false
};

query.IgnoreAuthors.Add("Microsoft");

IList<IPackageSearchMetadata> packages = await client.GetDependenciesAsync(query);


// Get latest version of a package
IPackageSearchMetadata package = await client.GetLatestAsync("Newtonsoft.Json");


// Get versions
IEnumerable<NuGetVersion> versions = await client.GetVersionsAsync("Newtonsoft.Json");
```

```csharp
// Check a package exists
bool exists = await client.ExistsAsync("Newtonsoft.Json");


// Check a package version exists
bool exists = await client.ExistsAsync("Newtonsoft.Json", "12.0.3");
```

```csharp
// Search for "json" packages (take first 100 records)
var request = new SearchPackagesRequest("json")
{
    Take = 100
};

IEnumerable<IPackageSearchMetadata> packages = await client.SearchAsync(request);
```
