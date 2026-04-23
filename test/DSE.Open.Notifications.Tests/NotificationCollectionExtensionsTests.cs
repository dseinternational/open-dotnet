// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Notifications.Tests;

public sealed class NotificationCollectionExtensionsTests
{
    [Fact]
    public void ToDiagnosticString()
    {
        var notifications = new List<Notification>(
        [
            Notification.Information("TEST123001", "A test (1)"),
            Notification.Information("TEST123002", "A test (2)")
        ]);

        var output = notifications.ToDiagnosticString();

        Assert.Equal("[ { Information (TEST123001): A test (1) }, { Information (TEST123002): A test (2) } ]", output);
    }

    [Fact]
    public void AnyCritical_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Notification>)null!).AnyCritical());
    }

    [Fact]
    public void AnyErrors_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Notification>)null!).AnyErrors());
    }

    [Fact]
    public void AnyWarnings_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Notification>)null!).AnyWarnings());
    }

    [Fact]
    public void NoErrors_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Notification>)null!).NoErrors());
    }

    [Fact]
    public void NoWarnings_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Notification>)null!).NoWarnings());
    }

    [Fact]
    public void ToDiagnosticString_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Notification>)null!).ToDiagnosticString());
    }

    [Fact]
    public void WhereErrorOrAbove_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((IEnumerable<Notification>)null!).WhereErrorOrAbove());
    }
}
