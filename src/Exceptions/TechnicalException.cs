using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 500 Internal Server Error for unexpected technical failures.
    /// </summary>
    /// <param name="message">The exception message (defaults to a generic contact-admin message).</param>
    /// <param name="messageHeader">An optional short header (defaults to "Internal server error").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class TechnicalException(
        string? message = null,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message ?? DefaultMessage, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessage = "Please contact the system administrator";
        private const string DefaultMessageHeader = "Internal server error";

        /// <summary>
        /// Initializes a new instance of <see cref="TechnicalException"/> with default message and header.
        /// </summary>
        public TechnicalException()
            : this(DefaultMessage, DefaultMessageHeader) { }

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    }
}
