using System.Net;

namespace DotNet.Library.Exceptions
{
    public class ForbiddenException(
        string message, 
        string? messageHeader = null, 
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Forbidden";

        public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
    }
}
