// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using AwesomeAssertions;

namespace DSE.Open.Sessions.Tests;

public class SessionContextSerializerTests
{
    [Fact]
    public void SerializeDeserializeUtf8Json()
    {
        var context = new SessionContext
        {
            StorageTokens =
            {
                ["test"] = "NbyaXkkUbDhmv6TSnPN1Himt5AukzZsAH22wsAISPXI74YhS8VKreWriNYAzAF80"
            }
        };

        var utf8Json = SessionContextSerializer.SerializeToUtf8Json(context);
        var deserialized = SessionContextSerializer.DeserializeFromUtf8Json(utf8Json.Span);
        _ = deserialized.Should().BeEquivalentTo(context);
    }

    [Theory]
    [InlineData(
        "eyJpZCI6InNlc3NfbHB0bFd5bnJjajN5Vk1xcHBkMzZCelh3aXBsZE0wRmlaTFF2M0diNk02ZU10c0NkIiwic3RvcmFnZV90b2tlbnMiOnt9LCJjcmVhdGVkIjoiMjAyMy0xMC0wNlQxMzoyMTozMS4yODI1MTIzKzAwOjAwIn0=")]
    public void TryDeserializeUtf8Json(string base64)
    {
        var succeeded = SessionContextSerializer.TryDeserializeFromBase64Utf8Json(base64, out var sessionContext);

        Assert.True(succeeded);
        Assert.NotNull(sessionContext);
    }

    [Fact]
    public void SerializeToBase64Utf8Json_ShouldRoundtrip()
    {
        // Arrange
        var context = new SessionContext
        {
            StorageTokens =
            {
                ["test"] = "NbyaXkkUbDhmv6TSnPN1Himt5AukzZsAH22wsAISPXI74YhS8VKreWriNYAzAF80"
            }
        };

        // Act
        var base64 = SessionContextSerializer.SerializeToBase64Utf8Json(context);
        var deserialized = SessionContextSerializer.DeserializeFromBase64Utf8Json(base64);

        // Assert
        Assert.Equivalent(context, deserialized);
    }

    [Fact]
    public void TryDeserializeFromBase64Utf8Json_WithValidValue_ShouldReturnTrue()
    {
        // Arrange
        var context = new SessionContext
        {
            StorageTokens =
            {
                ["test"] = "NbyaXkkUbDhmv6TSnPN1Himt5AukzZsAH22wsAISPXI74YhS8VKreWriNYAzAF80"
            }
        };

        // Act
        var base64 = SessionContextSerializer.SerializeToBase64Utf8Json(context);
        var result = SessionContextSerializer.TryDeserializeFromBase64Utf8Json(base64, out var sessionContext);

        // Assert
        Assert.True(result);
        Assert.NotNull(sessionContext);
        Assert.Equivalent(context, sessionContext);
    }

}
