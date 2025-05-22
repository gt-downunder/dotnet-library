using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class TechnicalException(string message = null, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message ?? DefaultMessage, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessage => "Please contact the system administrator";
        private static string DefaultMessageHeader => "Internal server error";

        public TechnicalException()
            : this(DefaultMessage, DefaultMessageHeader) { }

        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    }
}
