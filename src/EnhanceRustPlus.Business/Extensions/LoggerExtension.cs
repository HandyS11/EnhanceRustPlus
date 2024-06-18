using System.Diagnostics;
using EnhanceRustPlus.Business.Exceptions;
using Microsoft.Extensions.Logging;

namespace EnhanceRustPlus.Business.Extensions
{
    public static class LoggerExtension
    {
        public static void LogEnteringMethod(this ILogger logger)
        {
            var frame = new StackFrame(1);
            logger.LogDebug($"Entering {frame.GetMethod()?.DeclaringType?.Name} - {frame.GetMethod()?.Name}");
        }

        public static void LogExitingMethod(this ILogger logger)
        {
            var frame = new StackFrame(1);
            logger.LogDebug($"Exiting {frame.GetMethod()?.DeclaringType?.Name} - {frame.GetMethod()?.Name}");
        }

        public static void LogAndThrowBusinessException(this ILogger logger, string message, Exception e = null!)
        {
            var toThrow = new BusinessException(message, e);
            logger.LogError(toThrow, message);
            throw toThrow;
        }
    }
}
