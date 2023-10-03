// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using FluentAssertions;

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
}
