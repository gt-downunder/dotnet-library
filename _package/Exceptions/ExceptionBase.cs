using System.Net;

namespace DotNet.Library.Exceptions
{
    public abstract class ExceptionBase(
        string message, 
        string? messageHeader = null, 
        Exception? innerException = null)
        : Exception(message, innerException)
    {
        public abstract HttpStatusCode StatusCode { get; }

        public string MessageHeader { get; } = messageHeader ?? string.Empty;
    }
}
