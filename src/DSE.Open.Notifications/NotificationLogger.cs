// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Notifications;

/// <summary>
/// Logs <see cref="INotification"/> instances via an <see cref="ILogger"/> using the
/// source-generated <see cref="Log(ILogger, LogLevel, DiagnosticCode, string)"/> entry.
/// </summary>
public static partial class NotificationLogger
{
    /// <summary>
    /// Logs the notification at <see cref="LogLevel.Trace"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Logs the notification at <see cref="LogLevel.Debug"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Logs the notification at <see cref="LogLevel.Information"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Logs the notification at <see cref="LogLevel.Warning"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Logs the notification at <see cref="LogLevel.Error"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Logs the notification at <see cref="LogLevel.Critical"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Logs the notification using the explicitly supplied <paramref name="logLevel"/>,
    /// regardless of the notification's own <see cref="INotification.Level"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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
    /// Logs the notification, mapping its <see cref="INotification.Level"/> to the
    /// corresponding <see cref="LogLevel"/>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="notification">The notification to log.</param>
    /// <exception cref="ArgumentNullException"><paramref name="logger"/> or
    /// <paramref name="notification"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Source-generated logger method that writes a message of the form
    /// <c>[{diagnosticCode}] {message}</c> at the supplied <paramref name="level"/>.
    /// </summary>
    [LoggerMessage(message: "[{diagnosticCode}] {message}")]
    public static partial void Log(ILogger logger, LogLevel level, DiagnosticCode diagnosticCode, string message);
}
