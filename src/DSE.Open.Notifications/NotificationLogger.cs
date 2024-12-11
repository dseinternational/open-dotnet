// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Notifications;

public static partial class NotificationLogger
{
    public static void LogTrace(ILogger logger, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        Log(
            logger,
            LogLevel.Trace,
            notification.Code,
            notification.Message);
    }

    public static void LogDebug(ILogger logger, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        Log(
            logger,
            LogLevel.Debug,
            notification.Code,
            notification.Message);
    }

    public static void LogInformation(ILogger logger, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        Log(
            logger,
            LogLevel.Information,
            notification.Code,
            notification.Message);
    }

    public static void LogWarning(ILogger logger, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        Log(
            logger,
            LogLevel.Warning,
            notification.Code,
            notification.Message);
    }

    public static void LogError(ILogger logger, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        Log(
            logger,
            LogLevel.Error,
            notification.Code,
            notification.Message);
    }

    public static void LogCritical(ILogger logger, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        Log(
            logger,
            LogLevel.Critical,
            notification.Code,
            notification.Message);
    }

    public static void Log(ILogger logger, LogLevel logLevel, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        Log(
            logger,
            logLevel,
            notification.Code,
            notification.Message);
    }

    /// <summary>
    /// Logs the notification using the specified logger, mapping the notification level to the appropriate log level.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="notification"></param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the notification level is not recognised.</exception>
    public static void Log(ILogger logger, INotification notification)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(notification);

        var logLevel = notification.Level switch
        {
            NotificationLevel.Trace => LogLevel.Trace,
            NotificationLevel.Debug => LogLevel.Debug,
            NotificationLevel.Information => LogLevel.Information,
            NotificationLevel.Warning => LogLevel.Warning,
            NotificationLevel.Error => LogLevel.Error,
            NotificationLevel.Critical => LogLevel.Critical,
            _ => throw new ArgumentOutOfRangeException(nameof(notification), notification.Level, "Invalid notification level")
        };

        Log(
            logger,
            logLevel,
            notification.Code,
            notification.Message);
    }

    [LoggerMessage(message: "[{diagnosticCode}] {message}")]
    public static partial void Log(ILogger logger, LogLevel level, DiagnosticCode diagnosticCode, string message);
}
