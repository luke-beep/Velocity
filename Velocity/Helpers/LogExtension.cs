using NLog;
using Velocity.Models;

namespace Velocity.Helpers;

public class LogExtension
{
    public static Task Log(Logger logger, LogLevel logLevel, string message, LogEvent.EventIds eventId, Exception? e)
    {
        var logEvent = new LogEventInfo(logLevel, logger.Name, message)
        {
            Properties =
            {
                ["EventId"] = eventId,
                ["exception"] = e
            }
        };
        logger.Log(logEvent);
        return Task.CompletedTask;
    }
}