// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics;
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
    public void Create_InvalidLevel_ThrowsArgumentOutOfRange()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(
            () => new Notification("CODE123456", (NotificationLevel)999, "Message"));
    }

    [Fact]
    public void Create_NullMessage_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => new Notification("CODE123456", NotificationLevel.Information, null!));
    }

    [Fact]
    public void Create_EmptyMessage_Throws()
    {
        _ = Assert.Throws<ArgumentException>(
            () => new Notification("CODE123456", NotificationLevel.Information, string.Empty));
    }

    [Fact]
    public void Create_NullCodeString_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => new Notification((string)null!, NotificationLevel.Information, "Message"));
    }

    [Fact]
    public void Create_DiagnosticCodeOverload_Trace()
    {
        var code = new DiagnosticCode("CODE123456");
        var notification = Notification.Trace(code, "Message");
        Assert.Equal(code, notification.Code);
        Assert.Equal(NotificationLevel.Trace, notification.Level);
    }

    [Fact]
    public void Create_DiagnosticCodeOverload_Debug()
    {
        var code = new DiagnosticCode("CODE123456");
        var notification = Notification.Debug(code, "Message");
        Assert.Equal(code, notification.Code);
        Assert.Equal(NotificationLevel.Debug, notification.Level);
    }

    [Fact]
    public void Create_DiagnosticCodeOverload_Information()
    {
        var code = new DiagnosticCode("CODE123456");
        var notification = Notification.Information(code, "Message");
        Assert.Equal(code, notification.Code);
        Assert.Equal(NotificationLevel.Information, notification.Level);
    }

    [Fact]
    public void Create_DiagnosticCodeOverload_Warning()
    {
        var code = new DiagnosticCode("CODE123456");
        var notification = Notification.Warning(code, "Message");
        Assert.Equal(code, notification.Code);
        Assert.Equal(NotificationLevel.Warning, notification.Level);
    }

    [Fact]
    public void Create_DiagnosticCodeOverload_Error()
    {
        var code = new DiagnosticCode("CODE123456");
        var notification = Notification.Error(code, "Message");
        Assert.Equal(code, notification.Code);
        Assert.Equal(NotificationLevel.Error, notification.Level);
    }

    [Fact]
    public void Create_DiagnosticCodeOverload_Critical()
    {
        var code = new DiagnosticCode("CODE123456");
        var notification = Notification.Critical(code, "Message");
        Assert.Equal(code, notification.Code);
        Assert.Equal(NotificationLevel.Critical, notification.Level);
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
    public void Value_Equality_DifferentMessage_NotEqual()
    {
        var n1 = Notification.Warning("CODE123456", "Message A");
        var n2 = Notification.Warning("CODE123456", "Message B");
        Assert.NotEqual(n1, n2);
    }

    [Fact]
    public void Value_Equality_DifferentLevel_NotEqual()
    {
        var n1 = Notification.Warning("CODE123456", "Message");
        var n2 = Notification.Error("CODE123456", "Message");
        Assert.NotEqual(n1, n2);
    }

    [Fact]
    public void Value_Equality_DifferentCode_NotEqual()
    {
        var n1 = Notification.Warning("CODE123456", "Message");
        var n2 = Notification.Warning("CODE654321", "Message");
        Assert.NotEqual(n1, n2);
    }

    [Fact]
    public void ToString_Output()
    {
        var n1 = Notification.Warning("CODE123456", "Message");
        Assert.Equal("Warning (CODE123456): Message", n1.ToString());
    }

    [Theory]
    [InlineData(NotificationLevel.Trace)]
    [InlineData(NotificationLevel.Debug)]
    [InlineData(NotificationLevel.Information)]
    [InlineData(NotificationLevel.Warning)]
    [InlineData(NotificationLevel.Error)]
    [InlineData(NotificationLevel.Critical)]
    public void Serialize_Deserialize_RoundTrips_ForAllLevels(NotificationLevel level)
    {
        var n1 = new Notification("CODE123456", level, "Message");
        AssertJson.Roundtrip(n1);
    }

    [Fact]
    public void Serialize_Deserialize()
    {
        var n1 = Notification.Warning("CODE123456", "Message");
        AssertJson.Roundtrip(n1);
    }
}
