using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class ConflictException(string message, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessageHeader => "Conflict";

        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    }
}
