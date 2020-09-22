using System;
using NUnit.Framework;

namespace ByteDev.Nuget.UnitTests
{
    [TestFixture]
    public class NugetPackageNotFoundExceptionTests
    {
        [Test]
        public void WhenNoArgs_ThenSetMessageToDefault()
        {
            var sut = new NugetPackageNotFoundException();

            Assert.That(sut.Message, Is.EqualTo("Nuget package does not exist."));
        }

        [Test]
        public void WhenMessageSpecified_ThenSetMessage()
        {
            var sut = new NugetPackageNotFoundException("Some message.");

            Assert.That(sut.Message, Is.EqualTo("Some message."));
        }

        [Test]
        public void WhenMessageAndInnerExSpecified_ThenSetMessageAndInnerEx()
        {
            var innerException = new Exception();

            var sut = new NugetPackageNotFoundException("Some message.", innerException);

            Assert.That(sut.Message, Is.EqualTo("Some message."));
            Assert.That(sut.InnerException, Is.SameAs(innerException));
        }

        [Test]
        public void WhenIdAndVersionSpecified_ThenSetMessage()
        {
            var sut = new NugetPackageNotFoundException("Moq", "1.2.3");

            Assert.That(sut.Message, Is.EqualTo("Nuget package Moq 1.2.3 does not exist."));
        }
    }
}