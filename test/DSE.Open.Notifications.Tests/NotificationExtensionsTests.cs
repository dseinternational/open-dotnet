// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Notifications.Tests;

public sealed class NotificationExtensionsTests
{
    [Theory]
    [InlineData(NotificationLevel.Critical, true)]
    [InlineData(NotificationLevel.Error, false)]
    [InlineData(NotificationLevel.Warning, false)]
    [InlineData(NotificationLevel.Information, false)]
    [InlineData(NotificationLevel.Debug, false)]
    [InlineData(NotificationLevel.Trace, false)]
    public void IsCritical_ReturnsExpected(NotificationLevel level, bool expected)
    {
        var n = new Notification("CODE123456", level, "m");
        Assert.Equal(expected, n.IsCritical());
    }

    [Theory]
    [InlineData(NotificationLevel.Critical, true)]
    [InlineData(NotificationLevel.Error, true)]
    [InlineData(NotificationLevel.Warning, false)]
    [InlineData(NotificationLevel.Information, false)]
    [InlineData(NotificationLevel.Debug, false)]
    [InlineData(NotificationLevel.Trace, false)]
    public void IsErrorOrAbove_ReturnsExpected(NotificationLevel level, bool expected)
    {
        var n = new Notification("CODE123456", level, "m");
        Assert.Equal(expected, n.IsErrorOrAbove());
    }

    [Theory]
    [InlineData(NotificationLevel.Critical, true)]
    [InlineData(NotificationLevel.Error, true)]
    [InlineData(NotificationLevel.Warning, true)]
    [InlineData(NotificationLevel.Information, false)]
    [InlineData(NotificationLevel.Debug, false)]
    [InlineData(NotificationLevel.Trace, false)]
    public void IsWarningOrAbove_ReturnsExpected(NotificationLevel level, bool expected)
    {
        var n = new Notification("CODE123456", level, "m");
        Assert.Equal(expected, n.IsWarningOrAbove());
    }

    [Fact]
    public void IsCritical_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((Notification)null!).IsCritical());
    }

    [Fact]
    public void IsErrorOrAbove_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((Notification)null!).IsErrorOrAbove());
    }

    [Fact]
    public void IsWarningOrAbove_Null_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => ((Notification)null!).IsWarningOrAbove());
    }
}
