using System.Text;

namespace Shared.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Retrieves all messages from an exception, including messages from all nested inner exceptions, and formats them in
        ///     a readable way.
        /// </summary>
        /// <param name="exception">The exception to extract messages from.</param>
        /// <returns>
        ///     A formatted string containing the messages from the exception and all inner exceptions, if any.
        ///     Each message is prefixed with the exception level and includes the stack trace.
        /// </returns>
        /// <example>
        ///     <code>
        /// try
        /// {
        ///     // Some code that throws an exception
        /// }
        /// catch (Exception ex)
        /// {
        ///     Console.WriteLine(ex.GetAllMessages());
        /// }
        /// </code>
        /// </example>
        public static string GetAllMessages(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            StringBuilder exceptionMessage = new();
            Exception currentException = exception;
            int level = 0;

            // Traverse through the exception and all inner exceptions
            while (currentException != null)
            {
                exceptionMessage
                    .AppendLine($"Level {level}: {currentException.Message}")
                    .AppendLine($"Stack Trace: {currentException.StackTrace}")
                    .AppendLine(); // Blank line between levels for readability

                currentException = currentException.InnerException;
                level++;
            }

            return exceptionMessage.ToString();
        }
    }
}