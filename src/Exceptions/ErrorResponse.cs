
namespace Grondo.Exceptions
{
    /// <summary>
    /// Represents a standardized error response containing a message and header.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>Gets or sets the error message.</summary>
        public required string Message { get; set; }

        /// <summary>Gets or sets the error message header.</summary>
        public required string MessageHeader { get; set; }
    }
}
