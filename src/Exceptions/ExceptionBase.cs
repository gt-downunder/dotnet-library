using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Abstract base class for all Grondo domain exceptions.
    /// Provides a common <see cref="StatusCode"/> and <see cref="MessageHeader"/> for HTTP error responses.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header for UI display.</param>
    /// <param name="innerException">An optional inner exception.</param>
    public abstract class ExceptionBase(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : Exception(message, innerException)
    {
        /// <summary>Gets the HTTP status code associated with this exception.</summary>
        public abstract HttpStatusCode StatusCode { get; }

        /// <summary>Gets a short header suitable for display in error responses.</summary>
        public string MessageHeader { get; } = messageHeader ?? string.Empty;
    }
}
