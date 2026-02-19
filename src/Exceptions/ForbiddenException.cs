using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 403 Forbidden error.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Forbidden").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class ForbiddenException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Forbidden";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
    }
}
