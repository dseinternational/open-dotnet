// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;

namespace DSE.Open.Notifications.Tests;

public sealed class NotificationCollectionExtensionsTests
{
    private static readonly DiagnosticCode s_code = new("TEST123001");

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
    public void ToDiagnosticString_Empty_ReturnsBrackets()
    {
        var notifications = new List<Notification>();
        Assert.Equal("[ ]", notifications.ToDiagnosticString());
    }

    [Fact]
    public void ToDiagnosticString_Single()
    {
        var notifications = new List<Notification>
        {
            Notification.Warning("TEST123001", "Hello")
        };
        Assert.Equal("[ { Warning (TEST123001): Hello } ]", notifications.ToDiagnosticString());
    }

    [Fact]
    public void AddCritical_AppendsNotification()
    {
        var notifications = new List<Notification>();
        notifications.AddCritical(s_code, "crit");
        var n = Assert.Single(notifications);
        Assert.Equal(NotificationLevel.Critical, n.Level);
        Assert.Equal(s_code, n.Code);
        Assert.Equal("crit", n.Message);
    }

    [Fact]
    public void AddError_AppendsNotification()
    {
        var notifications = new List<Notification>();
        notifications.AddError(s_code, "err");
        var n = Assert.Single(notifications);
        Assert.Equal(NotificationLevel.Error, n.Level);
    }

    [Fact]
    public void AddWarning_AppendsNotification()
    {
        var notifications = new List<Notification>();
        notifications.AddWarning(s_code, "warn");
        var n = Assert.Single(notifications);
        Assert.Equal(NotificationLevel.Warning, n.Level);
    }

    [Fact]
    public void AddInformation_AppendsNotification()
    {
        var notifications = new List<Notification>();
        notifications.AddInformation(s_code, "info");
        var n = Assert.Single(notifications);
        Assert.Equal(NotificationLevel.Information, n.Level);
    }

#if DEBUG
    [Fact]
    public void AddDebug_AppendsNotificationInDebugBuild()
    {
        var notifications = new List<Notification>();
        notifications.AddDebug(s_code, "dbg");
        var n = Assert.Single(notifications);
        Assert.Equal(NotificationLevel.Debug, n.Level);
    }

    [Fact]
    public void AddTrace_AppendsNotificationInDebugBuild()
    {
        var notifications = new List<Notification>();
        notifications.AddTrace(s_code, "trace");
        var n = Assert.Single(notifications);
        Assert.Equal(NotificationLevel.Trace, n.Level);
    }
#endif

    [Fact]
    public void AddCritical_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ((ICollection<Notification>)null!).AddCritical(s_code, "m"));
    }

    [Fact]
    public void AddError_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ((ICollection<Notification>)null!).AddError(s_code, "m"));
    }

    [Fact]
    public void AddWarning_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ((ICollection<Notification>)null!).AddWarning(s_code, "m"));
    }

    [Fact]
    public void AddInformation_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ((ICollection<Notification>)null!).AddInformation(s_code, "m"));
    }

    [Fact]
    public void HasNotificationWithCode_WhenPresent_ReturnsTrue()
    {
        var notifications = new List<Notification>
        {
            Notification.Information("TEST123001", "a"),
            Notification.Warning("TEST123002", "b")
        };
        Assert.True(notifications.HasNotificationWithCode(new DiagnosticCode("TEST123002")));
    }

    [Fact]
    public void HasNotificationWithCode_WhenAbsent_ReturnsFalse()
    {
        var notifications = new List<Notification>
        {
            Notification.Information("TEST123001", "a")
        };
        Assert.False(notifications.HasNotificationWithCode(new DiagnosticCode("TEST999999")));
    }

    [Fact]
    public void HasNotificationWithCode_NullNotifications_ThrowsArgumentNullException()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ((IEnumerable<Notification>)null!).HasNotificationWithCode(s_code));
    }

    [Fact]
    public void AnyCritical_WhenCriticalPresent_ReturnsTrue()
    {
        var notifications = new List<Notification>
        {
            Notification.Warning("TEST123001", "a"),
            Notification.Critical("TEST123002", "b")
        };
        Assert.True(notifications.AnyCritical());
    }

    [Fact]
    public void AnyCritical_WhenOnlyErrors_ReturnsFalse()
    {
        var notifications = new List<Notification>
        {
            Notification.Error("TEST123001", "a")
        };
        Assert.False(notifications.AnyCritical());
    }

    [Fact]
    public void AnyErrors_WhenErrorPresent_ReturnsTrue()
    {
        var notifications = new List<Notification>
        {
            Notification.Warning("TEST123001", "a"),
            Notification.Error("TEST123002", "b")
        };
        Assert.True(notifications.AnyErrors());
    }

    [Fact]
    public void AnyErrors_WhenCriticalPresent_ReturnsTrue()
    {
        var notifications = new List<Notification>
        {
            Notification.Critical("TEST123002", "b")
        };
        Assert.True(notifications.AnyErrors());
    }

    [Fact]
    public void AnyErrors_WhenOnlyWarnings_ReturnsFalse()
    {
        var notifications = new List<Notification>
        {
            Notification.Warning("TEST123001", "a")
        };
        Assert.False(notifications.AnyErrors());
    }

    [Fact]
    public void AnyWarnings_WhenWarningPresent_ReturnsTrue()
    {
        var notifications = new List<Notification>
        {
            Notification.Information("TEST123001", "a"),
            Notification.Warning("TEST123002", "b")
        };
        Assert.True(notifications.AnyWarnings());
    }

    [Fact]
    public void AnyWarnings_WhenOnlyInformational_ReturnsFalse()
    {
        var notifications = new List<Notification>
        {
            Notification.Information("TEST123001", "a"),
            Notification.Trace("TEST123002", "b"),
            Notification.Debug("TEST123003", "c")
        };
        Assert.False(notifications.AnyWarnings());
    }

    [Fact]
    public void NoErrors_WithNoErrorOrCritical_ReturnsTrue()
    {
        var notifications = new List<Notification>
        {
            Notification.Information("TEST123001", "a"),
            Notification.Warning("TEST123002", "b")
        };
        Assert.True(notifications.NoErrors());
    }

    [Fact]
    public void NoErrors_WithError_ReturnsFalse()
    {
        var notifications = new List<Notification>
        {
            Notification.Error("TEST123001", "a")
        };
        Assert.False(notifications.NoErrors());
    }

    [Fact]
    public void NoWarnings_WithNoWarningErrorOrCritical_ReturnsTrue()
    {
        var notifications = new List<Notification>
        {
            Notification.Information("TEST123001", "a"),
            Notification.Trace("TEST123002", "b")
        };
        Assert.True(notifications.NoWarnings());
    }

    [Fact]
    public void NoWarnings_WithWarning_ReturnsFalse()
    {
        var notifications = new List<Notification>
        {
            Notification.Warning("TEST123001", "a")
        };
        Assert.False(notifications.NoWarnings());
    }

    [Fact]
    public void WhereErrorOrAbove_ReturnsOnlyErrorsAndCritical()
    {
        var notifications = new List<Notification>
        {
            Notification.Trace("TEST123001", "t"),
            Notification.Debug("TEST123002", "d"),
            Notification.Information("TEST123003", "i"),
            Notification.Warning("TEST123004", "w"),
            Notification.Error("TEST123005", "e"),
            Notification.Critical("TEST123006", "c")
        };

        var filtered = notifications.WhereErrorOrAbove().ToList();

        Assert.Equal(2, filtered.Count);
        Assert.All(filtered, n => Assert.True(n.Level >= NotificationLevel.Error));
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
