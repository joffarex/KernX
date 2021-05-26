using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace KernX.Logger
{
    public static class KXLoggerExtensions
    {
        // Make sure not to break in this function but one level higher during DEBUG
        [Conditional("DEBUG")]
        public static void Assert<T>(this ILogger<T> logger, bool condition, string message)
        {
            if (!condition)
            {
                logger.LogError("Assertion Failed: {Message}", message);
                if (Debugger.IsAttached)
                {
                    logger.LogError(new StackTrace().ToString());
                    Debugger.Break();
                }
            }
        }
    }
}