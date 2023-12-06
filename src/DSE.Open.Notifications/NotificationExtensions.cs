// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Notifications;

public static class NotificationExtensions
{
    public static bool IsCritical(this Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);
        return notification.Level == NotificationLevel.Critical;
    }

    public static bool IsErrorOrAbove(this Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);
        return notification.Level >= NotificationLevel.Error;
    }

    public static bool IsWarningOrAbove(this Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);
        return notification.Level >= NotificationLevel.Warning;
    }
}
