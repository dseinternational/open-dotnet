// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Notifications;
using FluentAssertions;

namespace DSE.Open.Results.Tests;

public class ResultTests
{
    [Fact]
    public void Serialize_Deserialize()
    {
        var r1 = new Result
        {
            Notifications =
            [
                Notification.Information("NTF123456", "Information"),
                Notification.Warning("NTF123456", "Warning"),
                Notification.Error("NTF123456", "Error")
            ]
        };
        var json = JsonSerializer.Serialize(r1);
        Assert.NotNull(json);
        var r2 = JsonSerializer.Deserialize<Result>(json);

        _ = r1.Should().BeEquivalentTo(r2);
    }

    [Fact]
    public void ResultIdIsNotDefault()
    {
        var request = new Result();
        Assert.NotEqual(default, request.ResultId);
    }
}
