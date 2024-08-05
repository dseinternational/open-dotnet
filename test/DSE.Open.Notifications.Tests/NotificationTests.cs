// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Notifications.Tests;

public class NotificationTests
{
    [Fact]
    public void Create_Error()
    {
        var notification = Notification.Error("CODE123456", "Message");
        Assert.Equal("CODE123456", notification.Code);
        Assert.Equal("Message", notification.Message);
        Assert.Equal(NotificationLevel.Error, notification.Level);
    }

    [Fact]
    public void Create_Information()
    {
        var notification = Notification.Information("CODE123456", "Message");
        Assert.Equal("CODE123456", notification.Code);
        Assert.Equal("Message", notification.Message);
        Assert.Equal(NotificationLevel.Information, notification.Level);
    }

    [Fact]
    public void Create_Trace()
    {
        var notification = Notification.Trace("CODE123456", "Message");
        Assert.Equal("CODE123456", notification.Code);
        Assert.Equal("Message", notification.Message);
        Assert.Equal(NotificationLevel.Trace, notification.Level);
    }

    [Fact]
    public void Create_Debug()
    {
        var notification = Notification.Debug("CODE123456", "Message");
        Assert.Equal("CODE123456", notification.Code);
        Assert.Equal("Message", notification.Message);
        Assert.Equal(NotificationLevel.Debug, notification.Level);
    }

    [Fact]
    public void Create_Critical()
    {
        var notification = Notification.Critical("CODE123456", "Message");
        Assert.Equal("CODE123456", notification.Code);
        Assert.Equal("Message", notification.Message);
        Assert.Equal(NotificationLevel.Critical, notification.Level);
    }

    [Fact]
    public void Create_Warning()
    {
        var notification = Notification.Warning("CODE123456", "Message");
        Assert.Equal("CODE123456", notification.Code);
        Assert.Equal("Message", notification.Message);
        Assert.Equal(NotificationLevel.Warning, notification.Level);
    }

    [Fact]
    public void Value_Equality()
    {
        var n1 = Notification.Warning("CODE123456", "Message");
        var n2 = n1 with { };
        Assert.NotSame(n1, n2);
        Assert.Equal(n1, n2);
    }

    [Fact]
    public void ToString_Output()
    {
        var n1 = Notification.Warning("CODE123456", "Message");
        Assert.Equal("Warning (CODE123456): Message", n1.ToString());
    }

    [Fact]
    public void Serialize_Deserialize()
    {
        var n1 = Notification.Warning("CODE123456", "Message");
        AssertJson.Roundtrip(n1);
    }
}
