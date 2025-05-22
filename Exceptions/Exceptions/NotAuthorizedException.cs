using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class NotAuthorizedException(string message, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessageHeader => "Not authorized";

        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    }
}
