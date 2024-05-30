// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Notifications;

public static class NotificationCollectionExtensions
{
    public static void AddCritical(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(Notification.Critical(code, message));
    }

    [Conditional("DEBUG")]
    public static void AddDebug(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(Notification.Debug(code, message));
    }

    public static void AddError(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(Notification.Error(code, message));
    }

    public static void AddInformation(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(Notification.Information(code, message));
    }

    public static void AddWarning(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(Notification.Warning(code, message));
    }

    public static void AddAndLog(this ICollection<Notification> notifications, Notification notification, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(notification);
        logger.LogNotification(notification);
    }

    public static void AddAndLogRange(this ICollection<Notification> notifications, IEnumerable<Notification> collection, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        Guard.IsNotNull(collection);

        foreach (var notification in collection)
        {
            notifications.AddAndLog(notification, logger);
        }
    }

    public static void AddAndLogTrace(this ICollection<Notification> notifications, Notification notification, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(notification);
        logger.LogTraceNotification(notification);
    }

    public static void AddAndLogDebug(this ICollection<Notification> notifications, Notification notification, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(notification);
        logger.LogDebugNotification(notification);
    }

    public static void AddAndLogInformation(this ICollection<Notification> notifications, Notification notification, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(notification);
        logger.LogInformationNotification(notification);
    }

    public static void AddAndLogWarning(this ICollection<Notification> notifications, Notification notification, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(notification);
        logger.LogWarningNotification(notification);
    }

    public static void AddAndLogError(this ICollection<Notification> notifications, Notification notification, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(notification);
        logger.LogErrorNotification(notification);
    }

    public static void AddAndLogCritical(this ICollection<Notification> notifications, Notification notification, ILogger logger)
    {
        Guard.IsNotNull(notifications);
        notifications.Add(notification);
        logger.LogCriticalNotification(notification);
    }

    /// <summary>
    /// Indicates if the notifications include any <see cref="NotificationLevel.Critical"/> level notifications.
    /// </summary>
    /// <param name="notifications"></param>
    /// <returns></returns>
    public static bool AnyCritical(this IEnumerable<Notification> notifications)
    {
        Guard.IsNotNull(notifications);
        return notifications.Any(n => n.IsCritical());
    }

    /// <summary>
    /// Indicates if the notifications include <see cref="NotificationLevel.Error"/> or
    /// <see cref="NotificationLevel.Critical"/> level notifications.
    /// </summary>
    /// <param name="notifications"></param>
    /// <returns><c>true</c> if the result notifications includes one or more
    /// <see cref="NotificationLevel.Error"/> or <see cref="NotificationLevel.Critical"/>
    /// level notifications.</returns>
    public static bool AnyErrors(this IEnumerable<Notification> notifications)
    {
        Guard.IsNotNull(notifications);
        return notifications.Any(n => n.IsErrorOrAbove());
    }

    /// <summary>
    /// Indicates if the notifications include <see cref="NotificationLevel.Warning"/> or
    /// <see cref="NotificationLevel.Error"/> or <see cref="NotificationLevel.Critical"/>
    /// level notifications.
    /// </summary>
    /// <param name="notifications"></param>
    /// <returns></returns>
    public static bool AnyWarnings(this IEnumerable<Notification> notifications)
    {
        Guard.IsNotNull(notifications);
        return notifications.Any(n => n.IsWarningOrAbove());
    }

    /// <summary>
    /// Indicates if the notifications do not include <see cref="NotificationLevel.Error"/> or
    /// <see cref="NotificationLevel.Critical"/> level notifications.
    /// </summary>
    /// <param name="notifications"></param>
    /// <returns></returns>
    public static bool NoErrors(this IEnumerable<Notification> notifications)
    {
        return !notifications.AnyErrors();
    }

    /// <summary>
    /// Indicates if the notifications do not include <see cref="NotificationLevel.Warning"/> or
    /// <see cref="NotificationLevel.Error"/> or <see cref="NotificationLevel.Critical"/>
    /// level notifications.
    /// </summary>
    /// <param name="notifications"></param>
    /// <returns></returns>
    public static bool NoWarnings(this IEnumerable<Notification> notifications)
    {
        return !notifications.AnyWarnings();
    }

    /// <summary>
    /// Generates a string report each notification in the collection on a new line.
    /// </summary>
    /// <param name="notifications"></param>
    /// <returns></returns>
    public static string ToDiagnosticString(this IEnumerable<Notification> notifications)
    {
        Guard.IsNotNull(notifications);

        var list = notifications.ToList();

        if (list.Count == 0)
        {
            return "[ ]";
        }

        var sb = new StringBuilder(list.Count * 128);

        _ = sb.Append("[ ");

        var i = 0;

        foreach (var n in list)
        {
            _ = sb.Append("{ ");
            _ = sb.Append(n);
            _ = sb.Append(" }");

            if (i < list.Count - 1)
            {
                _ = sb.Append(", ");
            }

            i++;
        }

        _ = sb.Append(" ]");

        return sb.ToString();
    }

    /*
     *
     * TODO: Move to DSE.Open.Results assembly
     *
    public static Result ToResult(this IEnumerable<Notification> notifications)
    {
        Guard.IsNotNull(notifications);
        return new Result { Notifications = notifications };
    }
    */

    public static IEnumerable<Notification> WhereErrorOrAbove(this IEnumerable<Notification> notifications)
    {
        Guard.IsNotNull(notifications);
        return notifications.Where(n => n.IsErrorOrAbove());
    }
}
