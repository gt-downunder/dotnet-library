using System.Globalization;
using System.Text;
using Grondo.Exceptions;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionEx
    {
        extension(Exception ex)
        {
            /// <summary>
            /// Produces a detailed string representation of the exception, including all inner exceptions
            /// and their stack traces.
            /// </summary>
            /// <returns>A multi-line string containing the exception type, message, and stack trace for each level.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the exception is <c>null</c>.</exception>
            public string ToDetailString()
            {
                ArgumentNullException.ThrowIfNull(ex);

                var sb = new StringBuilder();
                Exception? current = ex;

                for (int depth = 0; current is not null; depth++)
                {
                    if (depth > 0)
                    {
                        sb.AppendLine();
                        sb.AppendLine(CultureInfo.InvariantCulture, $"--- Inner Exception (depth {depth}) ---");
                    }

                    sb.AppendLine(CultureInfo.InvariantCulture, $"Type: {current.GetType().FullName}");
                    sb.AppendLine(CultureInfo.InvariantCulture, $"Message: {current.Message}");

                    if (!string.IsNullOrWhiteSpace(current.StackTrace))
                    {
                        sb.AppendLine(CultureInfo.InvariantCulture, $"StackTrace: {current.StackTrace}");
                    }

                    current = current.InnerException;
                }

                return sb.ToString().TrimEnd();
            }

            /// <summary>
            /// Flattens the exception and all its inner exceptions into a single sequence.
            /// </summary>
            /// <returns>A sequence containing the exception and all inner exceptions in order.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the exception is <c>null</c>.</exception>
            public IEnumerable<Exception> Flatten()
            {
                ArgumentNullException.ThrowIfNull(ex);

                Exception? current = ex;
                while (current is not null)
                {
                    yield return current;
                    current = current.InnerException;
                }
            }
        }

        extension(ExceptionBase exception)
        {
            /// <summary>
            /// Converts an <see cref="ExceptionBase"/>-derived exception into an <see cref="ErrorResponse"/>.
            /// </summary>
            /// <returns>An <see cref="ErrorResponse"/> containing the exception's message and header.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the exception is <c>null</c>.</exception>
            public ErrorResponse ToErrorResponse()
            {
                ArgumentNullException.ThrowIfNull(exception);

                return new ErrorResponse
                {
                    Message = exception.Message,
                    MessageHeader = exception.MessageHeader
                };
            }
        }
    }
}
