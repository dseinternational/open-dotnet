// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.ObjectModel;

namespace DSE.Open.Notifications;

public class NotificationCollection<T> : Collection<T>
    where T : INotification
{
    public NotificationCollection()
    {
    }

    public NotificationCollection(IEnumerable<T> collection)
        : this(collection.ToArray())
    {
    }

    public NotificationCollection(IList<T> list) : base(list)
    {
    }
}

public class NotificationCollection : NotificationCollection<Notification>
{
    public NotificationCollection()
    {
    }

    public NotificationCollection(IEnumerable<Notification> collection) : base(collection)
    {
    }

    public NotificationCollection(IList<Notification> list) : base(list)
    {
    }

    public static NotificationCollection Create(Notification notification) => new(new[] { notification });

    public static NotificationCollection Create(params Notification[] notifications) => new(notifications);
}
