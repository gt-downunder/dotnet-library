using System.Net;

namespace DotNet.Library.Exceptions
{
    public class DuplicateFoundException(
        string message, 
        string? messageHeader = null, 
        Exception? innerException = null)
        : ExceptionBase(message, messageHeader ?? DefaultMessageHeader, innerException)
    {
        private const string DefaultMessageHeader = "Duplicate found";

        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
    }
}
