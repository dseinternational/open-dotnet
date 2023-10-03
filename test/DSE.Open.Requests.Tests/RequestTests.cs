// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Sessions;
using FluentAssertions;

namespace DSE.Open.Requests.Tests;

public class RequestTests
{
    [Fact]
    public void RequestIdIsNotDefault()
    {
        var request = new Request();
        Assert.NotEqual(default, request.RequestId);
    }

    [Fact]
    public void SessionSetsDefault()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        var request = new Request { Session = new SessionContext() };
        Assert.True(request.Sessions.ContainsKey("default"));
#pragma warning restore CS0618 // Type or member is obsolete
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var request = new Request();
        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<Request>(json);
        Assert.NotNull(deserialized);
        _ = deserialized.Should().BeEquivalentTo(request);
    }
}
