// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Notifications;

public sealed class ReadOnlyNotificationValueCollection : ReadOnlyValueCollection<Notification>
{
    public static new readonly ReadOnlyNotificationValueCollection Empty = new();

    public ReadOnlyNotificationValueCollection()
    {
    }

    public ReadOnlyNotificationValueCollection(IEnumerable<Notification> list) : base(list)
    {
    }

    public override string ToString() => this.ToDiagnosticString();
}
