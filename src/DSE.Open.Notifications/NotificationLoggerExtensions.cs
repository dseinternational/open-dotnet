// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace DSE.Open.Notifications;

/// <summary>
/// <see cref="ILogger"/> extension methods that delegate to <see cref="NotificationLogger"/>
/// to log <see cref="INotification"/> instances.
/// </summary>
public static class NotificationLoggerExtensions
{
    /// <summary>
    /// Logs the notification using the explicitly supplied <paramref name="logLevel"/>,
    /// regardless of the notification's own <see cref="INotification.Level"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    public static void Log(this ILogger logger, LogLevel logLevel, INotification notification)
    {
        NotificationLogger.Log(logger, logLevel, notification);
    }

    /// <summary>Logs the notification at <see cref="LogLevel.Trace"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    public static void LogTrace(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogTrace(logger, notification);
    }

    /// <summary>Logs the notification at <see cref="LogLevel.Debug"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    public static void LogDebug(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogDebug(logger, notification);
    }

    /// <summary>Logs the notification at <see cref="LogLevel.Information"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    public static void LogInformation(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogInformation(logger, notification);
    }

    /// <summary>Logs the notification at <see cref="LogLevel.Warning"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    public static void LogWarning(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogWarning(logger, notification);
    }

    /// <summary>Logs the notification at <see cref="LogLevel.Error"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    public static void LogError(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogError(logger, notification);
    }

    /// <summary>Logs the notification at <see cref="LogLevel.Critical"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    public static void LogCritical(this ILogger logger, INotification notification)
    {
        NotificationLogger.LogCritical(logger, notification);
    }

    /// <summary>
    /// Logs the notification, mapping its <see cref="INotification.Level"/> to the
    /// corresponding <see cref="LogLevel"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the notification level is not recognised.</exception>
    public static void Log(this ILogger logger, INotification notification)
    {
        NotificationLogger.Log(logger, notification);
    }
}
