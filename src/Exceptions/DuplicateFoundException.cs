using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 409 Conflict error caused by a duplicate entity.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Duplicate found").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class DuplicateFoundException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Duplicate found";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    }
}
