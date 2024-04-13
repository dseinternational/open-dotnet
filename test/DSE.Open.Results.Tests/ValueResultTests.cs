// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Notifications;
using FluentAssertions;

namespace DSE.Open.Results.Tests;

public class ValueResultTests
{
    [Fact]
    public void Serialize_Deserialize()
    {
        var val = new ValueResult<string>
        {
            Value = "Test",
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ]
        };

        var json = JsonSerializer.Serialize(val);

        var val2 = JsonSerializer.Deserialize<ValueResult<string>>(json);

        _ = val.Should().BeEquivalentTo(val2);
    }

    [Fact]
    public void HasNotifications()
    {
        var val = new ValueResult<string>
        {
            Value = "Test",
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ]
        };

        Assert.True(val.HasNotifications);
    }

    [Fact]
    public void HasNotifications2()
    {
        var val = new ValueResult<string>
        {
            Value = "Test"
        };

        Assert.False(val.HasNotifications);
    }

    [Fact]
    public void HasAnyErrorNotifications()
    {
        var val = new ValueResult<string>
        {
            Value = "Test",
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ]
        };

        Assert.True(val.HasAnyErrorNotifications());
    }

    [Fact]
    public void HasAnyErrorNotifications2()
    {
        var val = new ValueResult<string>
        {
            Value = "Test",
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Critical("NTF123456", "Critical")
            ]
        };

        Assert.True(val.HasAnyErrorNotifications());
    }

    [Fact]
    public void HasAnyErrorNotifications3()
    {
        var val = new ValueResult<string>
        {
            Value = "Test",
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning")
            ]
        };

        Assert.False(val.HasAnyErrorNotifications());
    }

    [Fact]
    public void HasValueAndNoErrors()
    {
        var val = new ValueResult<string>
        {
            Value = "Test",
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning")
            ]
        };

        Assert.True(val.HasValueAndNoErrorNotifications());
    }

    [Fact]
    public void HasValueAndNoErrors2()
    {
        var val = new ValueResult<string>
        {
            Value = null,
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning")
            ]
        };

        Assert.False(val.HasValueAndNoErrorNotifications());
    }

    [Fact]
    public void Create()
    {
        var val1 = new ValueResult<string> { Value = "Test" };
        var val2 = ValueResult.Create("Test");

        Assert.Equal("Test", val1.Value);
        Assert.Equal("Test", val2.Value);
        Assert.False(val1.HasNotifications);
        Assert.False(val2.HasNotifications);
    }

    [Fact]
    public void Create_2()
    {
        var val1 = new ValueResult<string> { Value = "Test", Notifications = (Collections.Generic.ReadOnlyValueCollection<Notification>)([Notification.Information("NTF123456", "Test")]) };
        var val2 = ValueResult.Create("Test", [Notification.Information("NTF123456", "Test")]);

        Assert.Equal("Test", val1.Value);
        Assert.Equal("Test", val2.Value);
        Assert.True(val1.HasNotifications);
        Assert.True(val2.HasNotifications);
        Assert.Equal("Test", val1.Notifications[0].Message);
        Assert.Equal("Test", val2.Notifications[0].Message);
        Assert.Equal((Diagnostics.DiagnosticCode)"NTF123456", val1.Notifications[0].Code);
        Assert.Equal((Diagnostics.DiagnosticCode)"NTF123456", val2.Notifications[0].Code);
    }

    [Fact]
    public void Empty()
    {
        var val1 = new ValueResult<string> { Value = null };
        var val2 = ValueResult.Create<string>();
        var val3 = ValueResult<string>.Empty;

        Assert.Null(val1.Value);
        Assert.Null(val2.Value);
        Assert.Null(val3.Value);
        Assert.False(val1.HasNotifications);
        Assert.False(val2.HasNotifications);
        Assert.False(val3.HasNotifications);
    }
}
