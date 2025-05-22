using System;
using System.Net;

namespace DotNet.Library.Exceptions
{
    public class BadRequestException(string message, string messageHeader = null, Exception innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private static string DefaultMessageHeader => "Bad Request";

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
