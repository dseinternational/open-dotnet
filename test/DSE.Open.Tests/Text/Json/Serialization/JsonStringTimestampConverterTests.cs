// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Tests.Text.Json;

public class JsonStringTimestampConverterTests
{
    private static readonly Lazy<JsonSerializerOptions> s_options = new(() =>
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringTimestampConverter());
        return options;
    });

    [Theory]
    [InlineData("MTIzNDU2Nzg=")]
    [InlineData("AAAAAAAENic=")]
    [InlineData("AAAAAAAAD+Q=")]
    public void Serialize_Deserialize_Value(string timestamp)
    {
        // Arrange
        Span<byte> bytes = stackalloc byte[Timestamp.Size];
        _ = Convert.TryFromBase64String(timestamp, bytes, out _);
        var t1 = new Timestamp(bytes);

        // Act
        var j = JsonSerializer.Serialize(t1, s_options.Value);
        var t2 = JsonSerializer.Deserialize<Timestamp>(j, s_options.Value);

        // Assert
        Assert.Equal(t1, t2);
    }

    [Fact]
    public void Deserialize_WithValue_ShouldCorrectlyDeserialize()
    {
        // Arrange
        const string timestamp = "MTIzNDU2Nzg=";
        Span<byte> bytes = stackalloc byte[Timestamp.Size];
        _ = Convert.TryFromBase64String(timestamp, bytes, out _);
        var t1 = new Timestamp(bytes);

        // Act
        var t2 = JsonSerializer.Deserialize<Timestamp>($"\"{timestamp}\"", s_options.Value);

        // Assert
        Assert.Equal(t1, t2);
    }

    [Fact]
    public void Deserialize_WithEmptyValue_ShouldReturnEmptyTimestamp()
    {
        // Arrange
        var t1 = new Timestamp([0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0]);

        // Act
        var t2 = JsonSerializer.Deserialize<Timestamp>("\"\"", s_options.Value);

        // Assert
        Assert.Equal(t1, t2);
        Assert.Equal(Timestamp.Empty, t2);
    }
}
