// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Notifications.Tests;

public sealed class ReadOnlyNotificationValueCollectionTests
{
    [Fact]
    public void ToStringOutputsDiagnosticString()
    {
        var notifications = new ReadOnlyNotificationValueCollection(new[]
        {
            Notification.Information("TEST123001", "A test (1)"),
            Notification.Information("TEST123002", "A test (2)")
        });

        var output = notifications.ToString();

        Assert.Equal("[ { Information (TEST123001): A test (1) }, { Information (TEST123002): A test (2) } ]", output);
    }
}
