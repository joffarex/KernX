using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace KernX.Logger
{
    public static class KXLoggerExtensions
    {
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Assert<T>(this ILogger<T> logger, bool condition, string message)
        {
            if (!condition)
            {
                logger.LogError($"Assertion Failed: {message}");
                if (Debugger.IsAttached)
                {
                    logger.LogError(new StackTrace().ToString());
                    Debugger.Break();
                }
            }
        }
    }
}