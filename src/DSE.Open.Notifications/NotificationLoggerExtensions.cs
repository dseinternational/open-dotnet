// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace DSE.Open.Notifications;

public static class NotificationLoggerExtensions
{
    public static void Log(this ILogger logger, LogLevel logLevel, INotification notification)
    {
        NotificationLogger.Log(logger, logLevel, notification);
    }

    public static void LogTrace(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogTrace(logger, notification);
    }

    public static void LogDebug(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogDebug(logger, notification);
    }

    public static void LogInformation(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogInformation(logger, notification);
    }

    public static void LogWarning(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogWarning(logger, notification);
    }

    public static void LogError(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogError(logger, notification);
    }

    public static void LogCritical(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogCritical(logger, notification);
    }
}
