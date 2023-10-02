// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Notifications.Tests;

public sealed class NotificationCollectionExtensionsTests
{
    [Fact]
    public void ToDiagnosticString()
    {
        var notifications = new List<Notification>(new[]
        {
            Notification.Information("TEST123001", "A test (1)"),
            Notification.Information("TEST123002", "A test (2)")
        });

        var output = notifications.ToDiagnosticString();

        Assert.Equal("[ { Information (TEST123001): A test (1) }, { Information (TEST123002): A test (2) } ]", output);
    }
}
