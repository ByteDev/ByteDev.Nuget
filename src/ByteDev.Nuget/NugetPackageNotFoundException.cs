using System;
using System.Runtime.Serialization;

namespace ByteDev.Nuget
{
    /// <summary>
    /// Represents when a nuget package does not exist.
    /// </summary>
    [Serializable]
    public class NugetPackageNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.NugetPackageNotFoundException" /> class.
        /// </summary>
        public NugetPackageNotFoundException() : base("Nuget package does not exist.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.NugetPackageNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NugetPackageNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.NugetPackageNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>       
        public NugetPackageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.NugetPackageNotFoundException" /> class.
        /// </summary>
        /// <param name="packageId">Package ID.</param>
        /// <param name="packageVersion">Package version.</param>
        public NugetPackageNotFoundException(string packageId, string packageVersion) : this($"Nuget package {packageId} {packageVersion} does not exist.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Nuget.NugetPackageNotFoundException" /> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected NugetPackageNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}