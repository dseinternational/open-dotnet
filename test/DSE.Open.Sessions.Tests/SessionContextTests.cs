// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using AwesomeAssertions;

namespace DSE.Open.Sessions.Tests;

public class SessionContextTests
{
    [Fact]
    public void SerializeDeserializeJson()
    {
        var context = new SessionContext();
        context.StorageTokens["test"] = "NbyaXkkUbDhmv6TSnPN1Himt5AukzZsAH22wsAISPXI74YhS8VKreWriNYAzAF80";

        var json = JsonSerializer.Serialize(context);
        var deserialized = JsonSerializer.Deserialize<SessionContext>(json);
        _ = deserialized.Should().BeEquivalentTo(context);
    }

    [Fact]
    public void Merge()
    {
        var context = new SessionContext();
        context.StorageTokens["test"] = "NbyaXkkUbDhmv6TSnPN1Himt5AukzZsAH22wsAISPXI74YhS8VKreWriNYAzAF80";

        var context2 = new SessionContext();
        context2.Merge(context);

        Assert.True(context2.StorageTokens.ContainsKey("test"));
        Assert.Equal(context.StorageTokens["test"], context2.StorageTokens["test"]);
    }

    [Fact]
    public void Merge2()
    {
        var context = new SessionContext();
        context.StorageTokens["test"] = "NbyaXkkUbDhmv6TSnPN1Himt5AukzZsAH22wsAISPXI74YhS8VKreWriNYAzAF80";

        var context2 = new SessionContext();
        context2.StorageTokens["test"] = "py9M4VpHqfOb4x5y6KOJA64qQdnN6S5Olw5mzApllmk0w65Ezv8NuO8mWmhDzbpa";
        context2.Merge(context);

        Assert.True(context2.StorageTokens.ContainsKey("test"));
        Assert.Equal(context.StorageTokens["test"], context2.StorageTokens["test"]);
    }
}
