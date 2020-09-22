using System;
using NuGet.Versioning;
using NUnit.Framework;

namespace ByteDev.Nuget.UnitTests
{
    [TestFixture]
    public class NugetPackageFileNameTests
    {
        private const string ValidId = "Newtonsoft.Json";
        private readonly NuGetVersion ValidVersion = NuGetVersion.Parse("1.0.0");

        [TestFixture]
        public class Create_WithNuGetVersion : NugetPackageFileNameTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenIdIsNullOrEmpty_ThenThrowException(string id)
            {
                Assert.Throws<ArgumentException>(() => NugetPackageFileName.Create(id, ValidVersion));
            }

            [Test]
            public void WhenVersionIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => NugetPackageFileName.Create(ValidId, null as NuGetVersion));
            }

            [Test]
            public void WhenArgsValid_ThenReturnFileName()
            {
                var result = NugetPackageFileName.Create(ValidId, ValidVersion);

                Assert.That(result, Is.EqualTo("Newtonsoft.Json.1.0.0.nupkg"));
            }
        }

        [TestFixture]
        public class Create : NugetPackageFileNameTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenIdIsNullOrEmpty_ThenThrowException(string id)
            {
                Assert.Throws<ArgumentException>(() => NugetPackageFileName.Create(id, ValidVersion));
            }

            [TestCase(null)]
            [TestCase("")]
            public void WhenVersionIsNullOrEmpty_ThenThrowException(string version)
            {
                Assert.Throws<ArgumentException>(() => NugetPackageFileName.Create(ValidId, version));
            }

            [Test]
            public void WhenArgsValid_ThenReturnFileName()
            {
                var result = NugetPackageFileName.Create(ValidId, "1.0.0");

                Assert.That(result, Is.EqualTo("Newtonsoft.Json.1.0.0.nupkg"));
            }
        }
    }
}