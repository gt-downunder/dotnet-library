using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 400 Bad Request error.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Bad Request").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class BadRequestException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Bad Request";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
