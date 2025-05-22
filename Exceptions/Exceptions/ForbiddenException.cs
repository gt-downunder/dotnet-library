using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class ForbiddenException(string message, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessageHeader => "Forbidden";

        public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
    }
}
