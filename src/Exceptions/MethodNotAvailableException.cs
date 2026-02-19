using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 501 Not Implemented error.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Method not available").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class MethodNotAvailableException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Method not available";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.NotImplemented;
    }
}
