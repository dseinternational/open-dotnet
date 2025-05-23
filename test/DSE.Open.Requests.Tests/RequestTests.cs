// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using AwesomeAssertions;

namespace DSE.Open.Requests.Tests;

public class RequestTests
{
    [Fact]
    public void RequestIdIsNotNullOrWhiteSpace()
    {
        var request = new Request();
        Assert.False(string.IsNullOrWhiteSpace(request.RequestId));
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var request = new Request { Source = new Uri("https://testing.dsegroup.net") };
        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<Request>(json);
        Assert.NotNull(deserialized);
        _ = deserialized.Should().BeEquivalentTo(request);
    }
}
