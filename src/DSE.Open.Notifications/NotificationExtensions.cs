// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Notifications;

/// <summary>
/// Severity-level predicates for a single <see cref="Notification"/>.
/// </summary>
public static class NotificationExtensions
{
    /// <summary>
    /// Indicates whether the notification's <see cref="Notification.Level"/> is
    /// <see cref="NotificationLevel.Critical"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notification"/> is <see langword="null"/>.</exception>
    public static bool IsCritical(this Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);
        return notification.Level == NotificationLevel.Critical;
    }

    /// <summary>
    /// Indicates whether the notification's <see cref="Notification.Level"/> is
    /// <see cref="NotificationLevel.Error"/> or <see cref="NotificationLevel.Critical"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notification"/> is <see langword="null"/>.</exception>
    public static bool IsErrorOrAbove(this Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);
        return notification.Level >= NotificationLevel.Error;
    }

    /// <summary>
    /// Indicates whether the notification's <see cref="Notification.Level"/> is
    /// <see cref="NotificationLevel.Warning"/>, <see cref="NotificationLevel.Error"/> or
    /// <see cref="NotificationLevel.Critical"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="notification"/> is <see langword="null"/>.</exception>
    public static bool IsWarningOrAbove(this Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);
        return notification.Level >= NotificationLevel.Warning;
    }
}
