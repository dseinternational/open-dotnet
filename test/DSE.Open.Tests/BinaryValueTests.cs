// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;
using System.Text.Json;

namespace DSE.Open;

public class BinaryValueTests
{
    [Fact]
    public void Base62_RoundTrip_String()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();
            var encoded = value.ToBase62EncodedString();
            var value2 = BinaryValue.FromBase62(encoded);
            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void Base64_RoundTrip_String()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();
            var encoded = value.ToBase64EncodedString();
            var value2 = BinaryValue.FromBase64(encoded);
            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void HexLower_RoundTrip_String()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();
            var encoded = value.ToString(BinaryStringEncoding.HexLower);
            var value2 = BinaryValue.FromEncodedString(encoded, BinaryStringEncoding.HexLower);
            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void HexUpper_RoundTrip_String()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();
            var encoded = value.ToString(BinaryStringEncoding.HexUpper);
            var value2 = BinaryValue.FromEncodedString(encoded, BinaryStringEncoding.HexUpper);
            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void Base62_RoundTrip_Chars()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();

            var requiredLength = BinaryValue.GetRequiredBufferSize(value.Length, BinaryStringEncoding.Base62);

            var buffer = new char[requiredLength];

            var success = value.TryFormat(buffer, out var charsWritten, format: "b", provider: default);

            Assert.True(success);

            var value2 = BinaryValue.FromEncodedString(buffer.AsSpan(0, charsWritten).ToString(), BinaryStringEncoding.Base62);

            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void Base64_RoundTrip_Chars()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();

            var requiredLength = BinaryValue.GetRequiredBufferSize(value.Length, BinaryStringEncoding.Base64);

            var buffer = new char[requiredLength];

            var success = value.TryFormat(buffer, out var charsWritten, format: "B", provider: default);

            Assert.True(success);

            var value2 = BinaryValue.FromEncodedString(buffer.AsSpan(0, charsWritten).ToString(), BinaryStringEncoding.Base64);

            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void Base64_RoundTrip_Bytes()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();

            var requiredLength = BinaryValue.GetRequiredBufferSize(value.Length, BinaryStringEncoding.Base64);

            var buffer = new byte[requiredLength];

            var success = value.TryFormat(buffer, out var bytesWritten, format: "B", provider: default);

            Assert.True(success);

            var fromSuccess = BinaryValue.TryFromEncodedBytes(buffer.AsSpan(0, bytesWritten), BinaryStringEncoding.Base64, out var value2);

            Assert.True(fromSuccess);
            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void HexLower_RoundTrip_Chars()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();

            var requiredLength = BinaryValue.GetRequiredBufferSize(value.Length, BinaryStringEncoding.HexLower);

            var buffer = new char[requiredLength];

            var success = value.TryFormat(buffer, out var charsWritten, format: "x", provider: default);

            Assert.True(success);

            var value2 = BinaryValue.FromEncodedString(buffer.AsSpan(0, charsWritten).ToString(), BinaryStringEncoding.HexLower);

            Assert.Equal(value, value2);
        }
    }

    [Fact]
    public void FromHexBytes_WithValidHex_ShouldReturnCorrectData_Lower()
    {
        // Arrange
        var hex = "48656c6c6f2c20576f726c6421"u8; // "Hello, World!"

        // Act
        var result = BinaryValue.TryFromEncodedBytes(hex, BinaryStringEncoding.HexLower, out var decoded);
        var decodedHex = decoded.ToString(BinaryStringEncoding.HexLower);

        // Assert
        Assert.True(result);
        Assert.Equal(Encoding.UTF8.GetString(hex), decodedHex);
        Assert.True("Hello, World!"u8.SequenceEqual(Convert.FromHexString(decoded.ToString(BinaryStringEncoding.HexLower))));
    }

    [Fact]
    public void FromHexBytes_WithValidHex_ShouldReturnCorrectData_Upper()
    {
        // Arrange
        var hex = "48656C6C6F2C20576F726C6421"u8; // "Hello, World!"

        // Act
        var result = BinaryValue.TryFromEncodedBytes(hex, BinaryStringEncoding.HexUpper, out var decoded);
        var decodedHex = decoded.ToString(BinaryStringEncoding.HexUpper);

        // Assert
        Assert.True(result);
        Assert.Equal(Encoding.UTF8.GetString(hex), decodedHex);
        Assert.True("Hello, World!"u8.SequenceEqual(Convert.FromHexString(decodedHex)));
    }

    [Fact]
    public void HexUpper_RoundTrip_Chars()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = BinaryValue.GetRandomValue();

            var requiredLength = BinaryValue.GetRequiredBufferSize(value.Length, BinaryStringEncoding.HexUpper);

            var buffer = new char[requiredLength];

            var success = value.TryFormat(buffer, out var charsWritten, format: "X", provider: default);

            Assert.True(success);

            var value2 = BinaryValue.FromEncodedString(buffer.AsSpan(0, charsWritten).ToString(), BinaryStringEncoding.HexUpper);

            Assert.Equal(value, value2);
        }
    }

    [Theory]
    [InlineData("in/valid")]
    [InlineData("i~nvalid")]
    [InlineData("invalid*")]
    public void TryFromBase62EncodedString_fails_with_invalid_data(string encoded)
    {
        var succeeded = BinaryValue.TryFromBase62EncodedString(encoded, out _);
        Assert.False(succeeded);
    }

    [Theory]
    [InlineData("5h7Ao8e5qPCZNKJEoACLAvJ2bT6srtfrllMyoqdDrmBRhyxAQjyPBz5yz3NEJMAnSmTgFATqRUulAchYsMGm5q")]
    public void TryFromBase62EncodedString_succeeds_with_valid_data(string encoded)
    {
        var succeeded = BinaryValue.TryFromBase62EncodedString(encoded, out _);
        Assert.True(succeeded);
    }

    [Theory]
    [InlineData("5h7Ao8e5qPCZNKJEoACLAvJ2bT6srtfrllMyoqdDrmBRhyxAQjyPBz5yz3NEJMAnSmTgFATqRUulAchYsMGm5q")]
    public void TryFromEncodedString_succeeds_with_valid_data(string encoded)
    {
        var succeeded = BinaryValue.TryFromEncodedString(encoded, BinaryStringEncoding.Base62, out _);
        Assert.True(succeeded);
    }

    [Fact]
    public void TryFromBase64String_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var value = BinaryValue.GetRandomValue();
        var encoded = value.ToBase64EncodedString();

        // Act
        var result = BinaryValue.TryFromBase64EncodedString(encoded, out var decoded);

        // Assert
        Assert.True(result);
        Assert.Equal(value, decoded);
    }

    [Fact]
    public void TryFromHexLowerString_WithWithValidData_ShouldReturnTrue()
    {
        // Arrange
        var value = BinaryValue.GetRandomValue();
        var encoded = value.ToString(BinaryStringEncoding.HexLower);

        // Act
        var result = BinaryValue.TryFromEncodedString(encoded, BinaryStringEncoding.HexLower, out var decoded);

        // Assert
        Assert.True(result);
        Assert.Equal(value, decoded);
    }

    [Fact]
    public void TryFromHexUpperString_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var value = BinaryValue.GetRandomValue();
        var encoded = value.ToString(BinaryStringEncoding.HexUpper);

        // Act
        var result = BinaryValue.TryFromEncodedString(encoded, BinaryStringEncoding.HexUpper, out var decoded);

        // Assert
        Assert.True(result);
        Assert.Equal(value, decoded);
    }

    [Fact]
    public void ToString_WithNoArguments_ShouldFormatAsBase64()
    {
        // Arrange
        const string base64 = "SGVsbG8gV29ybGQh"; // "Hello World!"
        var value = BinaryValue.FromEncodedString("Hello World!");

        // Act
        var result = value.ToString();

        // Assert
        Assert.Equal(base64, result);
    }

    [Fact]
    public void Serialize_ShouldFormatAsBase64()
    {
        // Arrange
        const string base64 = "SGVsbG8gV29ybGQh"; // "Hello World!"
        var value = BinaryValue.FromEncodedString("Hello World!");

        // Act
        var result = JsonSerializer.Serialize(value);

        // Assert
        Assert.Equal($"\"{base64}\"", result);
    }

    [Fact]
    public void JsonSerialize_ShouldBeRoundtrippable()
    {
        // Arrange
        var value = BinaryValue.GetRandomValue();

        // Act
        var json = JsonSerializer.Serialize(value);
        var deserialized = JsonSerializer.Deserialize<BinaryValue>(json);

        // Assert
        Assert.Equal(value, deserialized);
    }

    [Fact]
    public void TryFormat_WithNoFormatString_ShouldFormatAsBase64()
    {
        // Arrange
        var value = BinaryValue.FromEncodedString("Hello World!");
        var buffer = new char[12 + 4]; // "Hello World!" + padding

        // Act
        var result = value.TryFormat(buffer, out var charsWritten, format: default, provider: default);

        // Assert
        Assert.True(result);
        Assert.Equal(16, charsWritten);
        Assert.True(buffer.AsSpan() is "SGVsbG8gV29ybGQh");
    }

    [Fact]
    public void ToString_WithNoFormat_ShouldFormatAsBase64()
    {
        // Arrange
        var value = BinaryValue.FromEncodedString("Hello World!");
        var buffer = new char[12 + 4]; // "Hello World!" + padding

        // Act
        var result = value.TryFormat(buffer, out var charsWritten, format: default, provider: default);

        // Assert
        Assert.True(result);
        Assert.Equal(16, charsWritten);
    }

    [Fact]
    public void GetRepeatableHashCode_ReturnsExpectedValue()
    {
        var value = BinaryValue.FromEncodedString("Hello World!");
        Assert.Equal(7439449919651422933u, value.GetRepeatableHashCode());
    }
}
