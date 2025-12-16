using System.Net;

namespace DotNet.Library.Exceptions
{
    public class TechnicalException(
        string? message = null, 
        string? messageHeader = null, 
        Exception? innerException = null)
        : ExceptionBase(message ?? DefaultMessage, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessage = "Please contact the system administrator";
        private const string DefaultMessageHeader = "Internal server error";

        public TechnicalException()
            : this(DefaultMessage, DefaultMessageHeader) { }

        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    }
}
