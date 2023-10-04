// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using FluentAssertions;

namespace DSE.Open.Requests.Tests;

public class RequestTests
{
    [Fact]
    public void RequestIdIsNotDefault()
    {
        var request = new Request();
        Assert.NotEqual(default, request.Id);
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
