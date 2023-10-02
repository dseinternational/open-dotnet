// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using FluentAssertions;

namespace DSE.Open.Sessions.Tests;

public class SessionContextSerializerTests
{
    [Fact]
    public void SerializeDeserializeUtf8Json()
    {
        var context = new SessionContext { StorageToken = "NbyaXkkUbDhmv6TSnPN1Himt5AukzZsAH22wsAISPXI74YhS8VKreWriNYAzAF80" };
        var utf8Json = SessionContextSerializer.SerializeToUtf8Json(context);
        var deserialized = SessionContextSerializer.DeserializeFromUtf8Json(utf8Json.Span);
        _ = deserialized.Should().BeEquivalentTo(context);
    }
}
