// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Text;

namespace DSE.Open.Notifications;

/// <summary>
/// Helpers for adding notifications to a collection and for querying sequences of
/// notifications by severity.
/// </summary>
public static class NotificationCollectionExtensions
{
    /// <summary>
    /// Adds a new <see cref="NotificationLevel.Critical"/> <see cref="Notification"/>
    /// to <paramref name="notifications"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static void AddCritical(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        notifications.Add(Notification.Critical(code, message));
    }

    /// <summary>
    /// Adds a new <see cref="NotificationLevel.Debug"/> <see cref="Notification"/>
    /// to <paramref name="notifications"/>.
    /// </summary>
    /// <remarks>
    /// This method is annotated with <see cref="ConditionalAttribute"/> for <c>DEBUG</c>, so all
    /// calls (including argument evaluation) are elided from non-DEBUG builds.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/> (DEBUG builds only).</exception>
    [Conditional("DEBUG")]
    public static void AddDebug(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        notifications.Add(Notification.Debug(code, message));
    }

    /// <summary>
    /// Adds a new <see cref="NotificationLevel.Error"/> <see cref="Notification"/>
    /// to <paramref name="notifications"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static void AddError(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        notifications.Add(Notification.Error(code, message));
    }

    /// <summary>
    /// Adds a new <see cref="NotificationLevel.Information"/> <see cref="Notification"/>
    /// to <paramref name="notifications"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static void AddInformation(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        notifications.Add(Notification.Information(code, message));
    }

    /// <summary>
    /// Adds a new <see cref="NotificationLevel.Trace"/> <see cref="Notification"/>
    /// to <paramref name="notifications"/>.
    /// </summary>
    /// <remarks>
    /// This method is annotated with <see cref="ConditionalAttribute"/> for <c>DEBUG</c>, so all
    /// calls (including argument evaluation) are elided from non-DEBUG builds.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/> (DEBUG builds only).</exception>
    [Conditional("DEBUG")]
    public static void AddTrace(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        notifications.Add(Notification.Trace(code, message));
    }

    /// <summary>
    /// Adds a new <see cref="NotificationLevel.Warning"/> <see cref="Notification"/>
    /// to <paramref name="notifications"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static void AddWarning(this ICollection<Notification> notifications, Diagnostics.DiagnosticCode code, string message)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        notifications.Add(Notification.Warning(code, message));
    }

    /// <summary>
    /// Indicates if the notifications include a notification with the specified
    /// <see cref="Diagnostics.DiagnosticCode"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static bool HasNotificationWithCode(this IEnumerable<Notification> notifications, Diagnostics.DiagnosticCode code)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        return notifications.Any(n => n.Code == code);
    }

    /// <summary>
    /// Indicates if the notifications include any <see cref="NotificationLevel.Critical"/> level notifications.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static bool AnyCritical(this IEnumerable<Notification> notifications)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        return notifications.Any(n => n.IsCritical());
    }

    /// <summary>
    /// Indicates if the notifications include <see cref="NotificationLevel.Error"/> or
    /// <see cref="NotificationLevel.Critical"/> level notifications.
    /// </summary>
    /// <returns><see langword="true"/> if the sequence contains one or more
    /// <see cref="NotificationLevel.Error"/> or <see cref="NotificationLevel.Critical"/>
    /// level notifications.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static bool AnyErrors(this IEnumerable<Notification> notifications)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        return notifications.Any(n => n.IsErrorOrAbove());
    }

    /// <summary>
    /// Indicates if the notifications include <see cref="NotificationLevel.Warning"/>,
    /// <see cref="NotificationLevel.Error"/> or <see cref="NotificationLevel.Critical"/>
    /// level notifications.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static bool AnyWarnings(this IEnumerable<Notification> notifications)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        return notifications.Any(n => n.IsWarningOrAbove());
    }

    /// <summary>
    /// Indicates if the notifications do not include any <see cref="NotificationLevel.Error"/>
    /// or <see cref="NotificationLevel.Critical"/> level notifications.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static bool NoErrors(this IEnumerable<Notification> notifications)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        return !notifications.Any(n => n.IsErrorOrAbove());
    }

    /// <summary>
    /// Indicates if the notifications do not include any <see cref="NotificationLevel.Warning"/>,
    /// <see cref="NotificationLevel.Error"/> or <see cref="NotificationLevel.Critical"/>
    /// level notifications.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static bool NoWarnings(this IEnumerable<Notification> notifications)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        return !notifications.Any(n => n.IsWarningOrAbove());
    }

    /// <summary>
    /// Generates a string report listing each notification in the collection within
    /// <c>[ ... ]</c> brackets and comma-separated <c>{ ... }</c> items. Returns
    /// <c>"[ ]"</c> for an empty sequence.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static string ToDiagnosticString(this IEnumerable<Notification> notifications)
    {
        ArgumentNullException.ThrowIfNull(notifications);

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
        ArgumentNullException.ThrowIfNull(notifications);
        return new Result { Notifications = notifications };
    }
    */

    /// <summary>
    /// Filters the sequence to include only notifications at <see cref="NotificationLevel.Error"/>
    /// or higher (i.e. <see cref="NotificationLevel.Error"/> and
    /// <see cref="NotificationLevel.Critical"/>).
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notifications"/> is
    /// <see langword="null"/>.</exception>
    public static IEnumerable<Notification> WhereErrorOrAbove(this IEnumerable<Notification> notifications)
    {
        ArgumentNullException.ThrowIfNull(notifications);
        return notifications.Where(n => n.IsErrorOrAbove());
    }
}
