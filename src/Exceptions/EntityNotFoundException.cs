using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 404 Not Found error.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Not found").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class EntityNotFoundException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Not found";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
