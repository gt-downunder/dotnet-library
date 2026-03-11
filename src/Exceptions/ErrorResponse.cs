
namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents a standardized error response containing a message and header.
    /// </summary>
    /// <param name="Message">The error message.</param>
    /// <param name="MessageHeader">The error message header.</param>
    public record ErrorResponse(string Message, string MessageHeader);
}
