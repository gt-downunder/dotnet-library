using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 409 Conflict error.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Conflict").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class ConflictException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Conflict";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    }
}
