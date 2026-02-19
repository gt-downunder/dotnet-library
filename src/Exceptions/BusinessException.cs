using System.Net;

namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents an HTTP 409 Conflict error caused by a business rule violation.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="messageHeader">An optional short header (defaults to "Business rule violation").</param>
    /// <param name="innerException">An optional inner exception.</param>
    public class BusinessException(
        string message,
        string? messageHeader = null,
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Business rule violation";

        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    }
}
