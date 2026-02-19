using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 401 Unauthorized error.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Not authorized").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class NotAuthorizedException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Not authorized";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    }
}
