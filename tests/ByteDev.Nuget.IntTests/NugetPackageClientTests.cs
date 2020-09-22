using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ByteDev.Collections;
using ByteDev.Nuget.Contract;
using ByteDev.Nuget.Factories;
using ByteDev.Testing.NUnit;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NUnit.Framework;

namespace ByteDev.Nuget.IntTests
{
    [TestFixture]
    public class NugetPackageClientTests
    {
        private const string ExistingPackageId = "ByteDev.Testing.NUnit";
        private const string ExistingPackageVersion = "1.1.0";

        private const string ExistingPackageNonExistingVersion = "9.0.0";

        private const string NonExistingPackageId = "NotExist04ef7821dde341e6962afe1aee13e437";
        
        private const string BasePath = @"C:\Temp\ByteDev.Nuget.IntTests";

        private readonly DirectoryInfo BasePathDirectory = new DirectoryInfo(BasePath);

        private NugetPackageClient _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new NugetPackageClient();
        }

        [TestFixture]
        public class GetAsync : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageNotExist_ThenReturnEmpty()
            {
                var result = await _sut.GetAsync(NonExistingPackageId);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public async Task WhenPackageExists_ThenReturnDetails()
            {
                var result = await _sut.GetAsync(ExistingPackageId);

                Assert.That(result.First().Identity.Id, Is.EqualTo(ExistingPackageId));
            }
        }

        [TestFixture]
        public class GetAsync_WithVersion : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageNotExist_ThenReturnNull()
            {
                var result = await _sut.GetAsync(NonExistingPackageId, ExistingPackageVersion);

                Assert.That(result, Is.Null);
            }

            [Test]
            public async Task WhenPackageExists_AndVersionDoesNotExist_ThenReturnNull()
            {
                var result = await _sut.GetAsync(ExistingPackageId, ExistingPackageNonExistingVersion);

                Assert.That(result, Is.Null);
            }

            [Test]
            public async Task WhenPackageExists_ThenReturnDetails()
            {
                var result = await _sut.GetAsync(ExistingPackageId, ExistingPackageVersion);

                Assert.That(result.Identity.Id, Is.EqualTo(ExistingPackageId));
            }
        }

        [TestFixture]
        public class GetLatestAsync : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageNotExist_ThenReturnNull()
            {
                var result = await _sut.GetLatestAsync(NonExistingPackageId);

                Assert.That(result, Is.Null);
            }

            [Test]
            public async Task WhenPackageExists_ThenReturnLatest()
            {
                var result = await _sut.GetLatestAsync("Newtonsoft.Json");

                Assert.That(result.Identity.Version.Version.Major, Is.GreaterThanOrEqualTo(12));
            }
        }

        [TestFixture]
        public class GetDependenciesAsync : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageHasNoDependencies_ThenReturnEmpty()
            {
                var query = new GetDependenciesRequest("ByteDev.Strings", "4.1.0", NuGetFrameworkFactory.CreateStandard("2.0"));

                var result = await _sut.GetDependenciesAsync(query);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public async Task WhenPackageHasSingleDependency_ThenReturnPackage()
            {
                var query = new GetDependenciesRequest("ByteDev.Crypto", "5.0.2", NuGetFrameworkFactory.CreateStandard("2.0"));

                var result = await _sut.GetDependenciesAsync(query);

                Assert.That(result.Single().Identity.ToString(), Is.EqualTo("ByteDev.Encoding.1.0.0"));
            }

            [Test]
            public async Task WhenPackageHasTwoDependencies_ThenReturnPackages()
            {
                var query = new GetDependenciesRequest("ByteDev.Hibp", "3.0.0", NuGetFrameworkFactory.CreateStandard("2.0"));

                var result = await _sut.GetDependenciesAsync(query);

                Assert.That(result.First().Identity.ToString(), Is.EqualTo("ByteDev.Common.7.0.0"));
                Assert.That(result.Second().Identity.ToString(), Is.EqualTo("Newtonsoft.Json.11.0.2"));
            }

            [Test]
            public async Task WhenPackageHasMultipleDependencies_ThenReturnPackages()
            {
                var query = new GetDependenciesRequest("ByteDev.Testing.NUnit", "1.1.0", NuGetFrameworkFactory.CreateStandard("2.0"));

                query.IgnoreAuthors.Add("Microsoft");

                var result = await _sut.GetDependenciesAsync(query);

                Assert.That(result.First().Identity.ToString(), Is.EqualTo("ByteDev.Crypto.5.0.2"));
                Assert.That(result.Second().Identity.ToString(), Is.EqualTo("ByteDev.Encoding.1.0.0"));
                Assert.That(result.Third().Identity.ToString(), Is.EqualTo("NUnit.3.12.0"));
            }

            [Test]
            public async Task WhenAddingRoot_ThenAddRootPackage()
            {
                var nuGetFramework = NuGetFrameworkFactory.CreateFramework("4.5");

                var query = new GetDependenciesRequest("Moq", "4.14.5", nuGetFramework)
                {
                    AddRootPackageToResults = true
                };

                query.IgnoreAuthors.Add("Microsoft");

                var result = await _sut.GetDependenciesAsync(query);

                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result.First().Identity.ToString(), Is.EqualTo("Moq.4.14.5"));
                Assert.That(result.Second().Identity.ToString(), Is.EqualTo("Castle.Core.4.4.0"));
            }
        }

        [TestFixture]
        public class GetVersionsAsync : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageDoesNotExist_ThenReturnEmpty()
            {
                var result = await _sut.GetVersionsAsync(NonExistingPackageId);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public async Task WhenPackageHasOneVersion_ThenReturnVersion()
            {
                IEnumerable<NuGetVersion> result = await _sut.GetVersionsAsync("ByteDev.ResourceIdentifier");

                Assert.That(result.IsSingle(), Is.True);
            }

            [Test]
            public async Task WhenPackageHasMultipleVersions_ThenReturnVersionsInfo()
            {
                var result = await _sut.GetVersionsAsync("ByteDev.DotNet");

                Assert.That(result.Count(), Is.GreaterThan(1));
            }
        }

        [TestFixture]
        public class SearchAsync : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackagesFound_ThenReturnPackages()
            {
                var result = await _sut.SearchAsync(new SearchRequest("bytedev")
                {
                    Take = 100
                });

                Assert.That(result.Count(), Is.EqualTo(30));
            }
        }

        [TestFixture]
        public class ExistsAsync : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageDoesNotExist_ThenReturnFalse()
            {
                var result = await _sut.ExistsAsync(NonExistingPackageId);

                Assert.That(result, Is.False);
            }

            [Test]
            public async Task WhenPackageDoesExist_ThenReturnTrue()
            {
                var result = await _sut.ExistsAsync(ExistingPackageId);

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class ExistsAsync_WithVersion : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageDoesNotExist_ThenReturnFalse()
            {
                var result = await _sut.ExistsAsync(ExistingPackageId, ExistingPackageNonExistingVersion);

                Assert.That(result, Is.False);
            }

            [Test]
            public async Task WhenPackageDoesExist_ThenReturnTrue()
            {
                var result = await _sut.ExistsAsync(ExistingPackageId, ExistingPackageVersion);

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        [NonParallelizable]
        public class DownloadToFileAsync : NugetPackageClientTests
        {
            [SetUp]
            public new void SetUp()
            {                
                BasePathDirectory.DeleteIfExists(true);
                BasePathDirectory.Create();
            }

            [OneTimeTearDown]
            public void ClassTearDown()
            {
                BasePathDirectory.DeleteIfExists(true);
            }

            [Test]
            public void WhenPackageDoesNotExist_ThenThrowException()
            {
                var fileName = NugetPackageFileName.Create(ExistingPackageId, ExistingPackageVersion);
                var filePath = Path.Combine(BasePath, fileName);

                Assert.ThrowsAsync<NugetPackageNotFoundException>(() => _sut.DownloadToFileAsync(NonExistingPackageId, ExistingPackageVersion, filePath));
            }

            [Test]
            public async Task WhenFileAlreadyExists_ThenOverWrite()
            {
                var fileName = NugetPackageFileName.Create(ExistingPackageId, ExistingPackageVersion);
                var filePath = Path.Combine(BasePath, fileName);

                TextFileTestBuilder.InFileSystem.WithSize(1).WithFilePath(filePath).Build();

                await _sut.DownloadToFileAsync(ExistingPackageId, ExistingPackageVersion, filePath);

                AssertFile.SizeGreaterThan(filePath, 1);
            }

            [Test]
            public async Task WhenPackageExists_ThenSaveLocally()
            {
                var fileName = NugetPackageFileName.Create(ExistingPackageId, ExistingPackageVersion);
                var filePath = Path.Combine(BasePath, fileName);

                await _sut.DownloadToFileAsync(ExistingPackageId, ExistingPackageVersion, filePath);

                AssertFile.Exists(filePath);
                AssertFile.SizeGreaterThan(filePath, 0);
            }

            // TODO: file already exists
        }

        [TestFixture]
        public class DownloadToStream : NugetPackageClientTests
        {
            [Test]
            public async Task WhenPackageExists_ThenReturnStream()
            {
                var result = await _sut.DownloadToStreamAsync(ExistingPackageId, ExistingPackageVersion);

                Assert.That(result.Length, Is.GreaterThan(0));

                await result.DisposeAsync();
            }

            [Test]
            public void WhenPackageDoesNotExist_ThenThrowException()
            {
                var ex = Assert.ThrowsAsync<NugetPackageNotFoundException>(() => _sut.DownloadToStreamAsync(NonExistingPackageId, ExistingPackageVersion));

                Assert.That(ex.Message, Is.EqualTo($"Nuget package {NonExistingPackageId} {ExistingPackageVersion} does not exist."));
            }
        }
    }
}