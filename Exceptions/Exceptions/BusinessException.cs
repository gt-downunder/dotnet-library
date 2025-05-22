using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class BusinessException(string message, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessageHeader => "Business rule violation";

        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    }
}
