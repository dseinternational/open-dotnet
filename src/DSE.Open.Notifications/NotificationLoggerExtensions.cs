// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace DSE.Open.Notifications;

public static class NotificationLoggerExtensions
{
    public static void LogNotification(this ILogger logger, INotification notification)
    {
        NotificationLogger.Log(logger, notification);
    }

    public static void LogNotification(this ILogger logger, LogLevel logLevel, INotification notification)
    {
        NotificationLogger.Log(logger, logLevel, notification);
    }

    public static void LogTraceNotification(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogTrace(logger, notification);
    }

    public static void LogDebugNotification(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogDebug(logger, notification);
    }

    public static void LogInformationNotification(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogInformation(logger, notification);
    }

    public static void LogWarningNotification(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogWarning(logger, notification);
    }

    public static void LogErrorNotification(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogError(logger, notification);
    }

    public static void LogCriticalNotification(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogCritical(logger, notification);
    }
}
