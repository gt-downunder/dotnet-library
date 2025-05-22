using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class EntityNotFoundException(string message, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessageHeader => "Not found";

        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
