// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Text.Json.Serialization;

public class JsonBinarySerializerTests
{
    [Fact]
    public void SerializeDeserializeString()
    {
        const string value = "A string value";
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromUtf8Json<string>(utf8Bytes.Span);
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeInt32()
    {
        const int value = 123456;
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromUtf8Json<int>(utf8Bytes.Span);
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeInt32Array()
    {
        int[] value = [1, 2, 3, 4, 123456];
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromUtf8Json<int[]>(utf8Bytes.Span);
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeToUtf8Json_WithNoOptionsSpecified_ShouldUseRelaxedJsonEscaping()
    {
        // Arrange
        const string value = "<script>alert('hello');</script>";

        // Act
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);

        // Assert
        Assert.Equal("\"<script>alert('hello');</script>\"", Encoding.UTF8.GetString(utf8Bytes.Span));
    }

    [Fact]
    public void SerializeToBase64Utf8Json_WithNoOptions_ShouldWriteUnescapedBase64()
    {
        // Arrange
        const string value = "<p>A text value</p>";
        var quotedValue = "\"<p>A text value</p>\""u8;
        var expectedValue = Convert.ToBase64String(quotedValue);

        // Act
        var base64 = JsonBinarySerializer.SerializeToBase64Utf8Json(value);

        // Assert
        Assert.Equal(expectedValue, base64);
    }

    [Fact]
    public void SerializeToBase64_WithEscapingOptions_ShouldEscape()
    {
        // Arrange
        const string value = "<p>A text value</p>";

        // Act
        var base64 = JsonBinarySerializer.SerializeToBase64Utf8Json(value, JsonSerializerOptions.Default);

        // Assert
        // Can't really test the escaping, but we can test that it's changed the value
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        Assert.NotEqual(value, decoded);
    }

    [Theory]
    [MemberData(nameof(JsonSerializerOptionsData))]
    public void Utf8Serialize_RoundTrip(JsonSerializerOptions? options)
    {
        // Arrange
        var value = new Uri("https://www.example.com/home/sub?query=1&query=2#fragment");

        // Act
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromUtf8Json<Uri>(utf8Bytes.Span, options);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Theory]
    [MemberData(nameof(JsonSerializerOptionsData))]
    public void Base64Utf8Serialize_RoundTrip(JsonSerializerOptions? options)
    {
        // Arrange
        var value = new Uri("https://www.example.com/home/sub?query=1&query=2#fragment");

        // Act
        var base64 = JsonBinarySerializer.SerializeToBase64Utf8Json(value);
        var deserializedValue = JsonBinarySerializer.DeserializeFromBase64Utf8Json<Uri>(base64, options);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Theory]
    [MemberData(nameof(JsonSerializerOptionsData))]
    public void TryDeserializeFromUtf8Json_WithValidJson_ShouldReturnValue(JsonSerializerOptions? options)
    {
        // Arrange
        var value = new Uri("https://www.example.com/home/sub?query=1&query=2#fragment");
        var utf8Bytes = JsonBinarySerializer.SerializeToUtf8Json(value);

        // Act
        var result = JsonBinarySerializer.TryDeserializeFromUtf8Json(utf8Bytes.Span, options, out Uri? deserializedValue);

        // Assert
        Assert.True(result);
        Assert.Equal(value, deserializedValue);
    }

    [Theory]
    [MemberData(nameof(JsonSerializerOptionsData))]
    public void TryDeserializeFromUtf8Json_WithInvalidJson_ShouldReturnFalse(JsonSerializerOptions? options)
    {
        // Arrange
        var utf8Bytes = "invalid json"u8;

        // Act
        var result = JsonBinarySerializer.TryDeserializeFromUtf8Json(utf8Bytes, options, out Uri? deserializedValue);

        // Assert
        Assert.False(result);
        Assert.Null(deserializedValue);
    }

    [Theory]
    [MemberData(nameof(JsonSerializerOptionsData))]
    public void TryDeserializeFromBase64Utf8Json_WithValidJson_ShouldReturnValue(JsonSerializerOptions? options)
    {
        // Arrange
        var value = new Uri("https://www.example.com/home/sub?query=1&query=2#fragment");
        var base64 = JsonBinarySerializer.SerializeToBase64Utf8Json(value);

        // Act
        var result = JsonBinarySerializer.TryDeserializeFromBase64Utf8Json(base64, options, out Uri? deserializedValue);

        // Assert
        Assert.True(result);
        Assert.Equal(value, deserializedValue);
    }

    [Theory]
    [MemberData(nameof(JsonSerializerOptionsData))]
    public void TryDeserializeFromBase64Utf8Json_WithInvalidJson_ShouldReturnFalse(JsonSerializerOptions? options)
    {
        // Arrange
        const string base64 = "invalid json";

        // Act
        var result = JsonBinarySerializer.TryDeserializeFromBase64Utf8Json(base64, options, out Uri? deserializedValue);

        // Assert
        Assert.False(result);
        Assert.Null(deserializedValue);
    }

    [Fact]
    public void TryDeserializeFromBase64Utf8Json_WithLongValidValue_ShouldDeserialize()
    {
        // Arrange
        // inverse of 3 * Math.Ceiling(base64.Length / 4)
        var str = string.Create(MemoryThresholds.StackallocByteThreshold / 3 * 4, 'a', (span, value) => span.Fill(value));
        var base64 = JsonBinarySerializer.SerializeToBase64Utf8Json(str);

        // Act
        var result = JsonBinarySerializer.TryDeserializeFromBase64Utf8Json(base64, out string? deserializedValue);

        // Assert
        Assert.True(result);
        Assert.Equal(str, deserializedValue);
    }

    public static IEnumerable<object?[]> JsonSerializerOptionsData()
    {
        yield return [null];
        yield return [JsonSerializerOptions.Default];

        yield return
        [
            new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }
        ];

        yield return [JsonSharedOptions.UnicodeRangesAll];
        yield return [JsonSharedOptions.RelaxedJsonEscaping];
    }
}
